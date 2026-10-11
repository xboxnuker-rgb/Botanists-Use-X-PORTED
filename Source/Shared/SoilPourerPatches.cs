#if FEATURE_SOIL
using System.Collections;
using HarmonyLib;
using Il2CppFishNet;
using Il2CppFishNet.Connection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne;
using Il2CppScheduleOne.AvatarFramework.Equipping;
using Il2CppScheduleOne.Employees;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.Management;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using UnityEngine;

namespace BotanistsUseX.Runtime;

internal static class SoilPourerAutomation
{
    private const float RefillCheckIntervalSeconds = 1f;
    private static readonly Dictionary<IntPtr, float> NextRefillCheckByBotanist = new();

    internal static bool TryGetContext(
        GrowContainer? container,
        out Pot? pot,
        out SoilPourer? pourer)
    {
        pot = container?.TryCast<Pot>();
        pourer = pot is null ? null : DeviceLocator.FindSoilPourer(pot);
        return container is not null && pot is not null && pourer is not null;
    }

    internal static bool TryGetContext(
        AddSoilToGrowContainerBehaviour behaviour,
        out GrowContainer? container,
        out Pot? pot,
        out SoilPourer? pourer)
    {
        container = behaviour._growContainer;
        return TryGetContext(container, out pot, out pourer);
    }

    internal static bool ShouldRefill(GrowContainer container, SoilPourer pourer)
    {
        int charges = SoilPourerChargeStore.GetCharges(pourer);
        return charges < RuntimeSettings.SoilPourerCapacity &&
            (charges == 0 || container.IsFullyFilledWithSoil);
    }

    internal static Il2CppStringArray? GetSelectedSeedIDs(Pot pot)
    {
        Il2CppStringArray? seedIDs = pot.potConfiguration?.GetSelectedSeedIDs();
        return seedIDs is not null && seedIDs.Length > 0 ? seedIDs : null;
    }

    internal static Il2CppStringArray GetStoredSoilID(SoilPourer pourer)
    {
        Il2CppStringArray result = new(1);
        result[0] = pourer.SoilID;
        return result;
    }

    internal static bool TryApplySoil(
        GrowContainer container,
        SoilPourer pourer,
        ItemInstance? usedItem)
    {
        int charges = SoilPourerChargeStore.GetCharges(pourer);
        SoilDefinition? soil = charges > 0
            ? Registry.GetItem<SoilDefinition>(pourer.SoilID)
            : usedItem?.Definition?.TryCast<SoilDefinition>();

        if (soil is null)
        {
            RuntimeSettings.Warning("An attached soil pourer was found, but no valid soil definition was available; falling back to the game's normal action.");
            return false;
        }

        pourer.ActivateSound?.Play();
        container.SetSoil(soil);
        container.SetSoilAmount(container.SoilCapacity);
        container.SetRemainingSoilUses(soil.Uses);
        container.SyncSoilData();
        int remainingCharges = SoilPourerChargeStore.ConsumeCharge(pourer);

        RuntimeSettings.Debug(
            $"Used soil pourer for grow container at {container._originCoordinate}; " +
            $"{remainingCharges} charge(s) remain.");
        return true;
    }

    internal static void TryAssignIdleRefill(Botanist botanist)
    {
        LoadManager? loadManager = LoadManager.Instance;
        if (!RuntimeSettings.AutoRefillSoilPourers ||
            loadManager is null ||
            loadManager.IsLoading ||
            !loadManager.IsGameLoaded ||
            loadManager.TimeSinceGameLoaded < 5f ||
            botanist.IsAnyWorkInProgress())
        {
            return;
        }

        float now = UnityEngine.Time.realtimeSinceStartup;
        if (NextRefillCheckByBotanist.TryGetValue(botanist.Pointer, out float nextCheck) && now < nextCheck)
        {
            return;
        }

        NextRefillCheckByBotanist[botanist.Pointer] = now + RefillCheckIntervalSeconds;

        BotanistConfiguration? configuration = botanist.Configuration?.TryCast<BotanistConfiguration>();
        AddSoilToGrowContainerBehaviour? behaviour = botanist._addSoilToGrowContainerBehaviour;
        if (configuration?.AssignedPots is null || behaviour is null)
        {
            return;
        }

        foreach (Pot pot in configuration.AssignedPots)
        {
            if (pot is null || pot.CurrentSoil is null ||
                !TryGetContext(pot, out _, out SoilPourer? pourer) ||
                SoilPourerChargeStore.IsFull(pourer!) ||
                !ShouldRefill(pot, pourer!) ||
                !behaviour.AreTaskConditionsMetForContainer(pot))
            {
                continue;
            }

            botanist.SetIdle(false);
            behaviour.AssignAndEnable(pot);
            NextRefillCheckByBotanist[botanist.Pointer] = now + 15f;
            RuntimeSettings.Debug(
                $"Assigned idle soil-pourer refill at {pourer!._originCoordinate}; " +
                $"current charge(s): {SoilPourerChargeStore.GetCharges(pourer)}.");
            return;
        }
    }

    internal static void MarkRefillCompleted(Botanist? botanist)
    {
        if (botanist is not null)
        {
            NextRefillCheckByBotanist[botanist.Pointer] =
                UnityEngine.Time.realtimeSinceStartup + RefillCheckIntervalSeconds;
        }
    }

    internal static bool TryGetManualTopUp(
        SoilPourer pourer,
        out PlayerInventory? inventory,
        out ItemInstance? soilItem)
    {
        inventory = PlayerInventory.Instance;
        soilItem = inventory?.EquippedItem;

        if (!RuntimeSettings.AllowManualMultiBagFill ||
            inventory is null ||
            !inventory.isAnythingEquipped ||
            soilItem?.Definition?.TryCast<SoilDefinition>() is null)
        {
            return false;
        }

        int charges = SoilPourerChargeStore.GetCharges(pourer);
        return charges > 0 &&
            charges < RuntimeSettings.SoilPourerCapacity &&
            string.Equals(soilItem.ID, pourer.SoilID, StringComparison.OrdinalIgnoreCase);
    }

    internal static bool TryStartMultiChargeHandleDispense(SoilPourer pourer)
    {
        int startingCharges = SoilPourerChargeStore.GetCharges(pourer);
        if (RuntimeSettings.SoilPourerCapacity <= 1 ||
            pourer.isDispensing ||
            startingCharges <= 0)
        {
            return false;
        }

        SoilDefinition? soil = Registry.GetItem<SoilDefinition>(pourer.SoilID);
        Pot? targetPot = null;

        try
        {
            foreach (Pot pot in pourer.GetPots())
            {
                if (pot is null || pot.IsFullyFilledWithSoil ||
                    (pot.CurrentSoil is not null &&
                        !string.Equals(pot.CurrentSoil.ID, pourer.SoilID, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                targetPot = pot;
                break;
            }
        }
        catch (Exception exception)
        {
            RuntimeSettings.Warning(
                $"Could not resolve a pot for multi-charge soil dispensing: {exception.Message}");
        }

        if (soil is null)
        {
            return false;
        }

        int remainingCharges = SoilPourerChargeStore.ReserveChargeForCustomDispense(pourer);
        if (remainingCharges < 0)
        {
            return false;
        }

        SoilPourerChargeStore.BroadcastDispenseStarted(
            pourer,
            startingCharges,
            remainingCharges);

        RuntimeSettings.Debug(
            $"Starting custom soil-pourer dispense at {pourer._originCoordinate}: " +
            $"{startingCharges}/{RuntimeSettings.SoilPourerCapacity} to " +
            $"{remainingCharges}/{RuntimeSettings.SoilPourerCapacity}; " +
            $"eligible pot: {(targetPot is null ? "none" : targetPot._originCoordinate.ToString())}.");

        MelonCoroutines.Start(
            DispenseConfiguredCharge(
                pourer,
                targetPot,
                soil,
                startingCharges,
                remainingCharges));
        return true;
    }

    private static IEnumerator DispenseConfiguredCharge(
        SoilPourer pourer,
        Pot? pot,
        SoilDefinition soil,
        int startingCharges,
        int remainingCharges)
    {
        pourer.isDispensing = true;
        pourer.ActivateSound?.Play();
        pourer.PourParticles?.Play();
        pourer.PourAnimation?.Play();
        pourer.DirtPourSound?.Play();

        float startingSoilAmount = 0f;
        if (pot is not null)
        {
            startingSoilAmount = pot.CurrentSoil is null
                ? 0f
                : pot.NormalizedSoilAmount * pot.SoilCapacity;

            pot.SetSoil(soil);
            pot.SetSoilState(Pot.ESoilState.Flat);
            pot.SetRemainingSoilUses(soil.Uses);
            pot.SetSoilAmount(startingSoilAmount);
        }

        float duration = Math.Max(0.1f, pourer.AnimationDuration);
        float elapsed = 0f;
        float startLevel = startingCharges / (float)RuntimeSettings.SoilPourerCapacity;
        float targetLevel = remainingCharges / (float)RuntimeSettings.SoilPourerCapacity;

        while (elapsed < duration)
        {
            yield return new WaitForEndOfFrame();

            if (pourer is null || pourer.Pointer == IntPtr.Zero)
            {
                yield break;
            }

            elapsed += Time.deltaTime;
            float progress = Math.Clamp(elapsed / duration, 0f, 1f);
            pourer.SetSoilLevel(Mathf.Lerp(startLevel, targetLevel, progress));
            if (pot is not null && pot.Pointer != IntPtr.Zero)
            {
                pot.SetSoilAmount(Mathf.Lerp(startingSoilAmount, pot.SoilCapacity, progress));
            }
        }

        pourer.SetSoilLevel(targetLevel);
        if (pot is not null && pot.Pointer != IntPtr.Zero)
        {
            pot.SetSoilAmount(pot.SoilCapacity);
            pot.SyncSoilData();
        }

        if (remainingCharges == 0)
        {
            pourer.SendSoil(string.Empty);
        }
        else
        {
            SoilPourerChargeStore.BroadcastCharges(pourer, remainingCharges);
        }

        pourer.PourParticles?.Stop();
        pourer.isDispensing = false;
        if (remainingCharges > 0)
        {
            SoilPourerChargeStore.RefreshManualFillInteraction(pourer);
        }

        RuntimeSettings.Debug(
            $"Custom soil-pourer dispense completed at {pourer._originCoordinate}; " +
            $"{remainingCharges} charge(s) remain; " +
            $"pot: {(pot is null ? "none" : pot._originCoordinate.ToString())}.");

        yield return new WaitForSeconds(1f);
        pourer.DirtPourSound?.Stop();
    }

    internal static void StartRemoteDispenseVisual(
        SoilPourer pourer,
        int startingCharges,
        int remainingCharges)
    {
        MelonCoroutines.Start(RemoteDispenseVisual(
            pourer,
            startingCharges,
            remainingCharges));
    }

    private static IEnumerator RemoteDispenseVisual(
        SoilPourer pourer,
        int startingCharges,
        int remainingCharges)
    {
        pourer.isDispensing = true;
        pourer.ActivateSound?.Play();
        pourer.PourParticles?.Play();
        pourer.PourAnimation?.Play();
        pourer.DirtPourSound?.Play();

        float duration = Math.Max(0.1f, pourer.AnimationDuration);
        float elapsed = 0f;
        float startLevel = startingCharges / (float)RuntimeSettings.SoilPourerCapacity;
        float targetLevel = remainingCharges / (float)RuntimeSettings.SoilPourerCapacity;

        while (elapsed < duration)
        {
            yield return new WaitForEndOfFrame();
            if (pourer is null || pourer.Pointer == IntPtr.Zero)
            {
                yield break;
            }

            elapsed += Time.deltaTime;
            pourer.SetSoilLevel(Mathf.Lerp(
                startLevel,
                targetLevel,
                Math.Clamp(elapsed / duration, 0f, 1f)));
        }

        pourer.SetSoilLevel(targetLevel);
        pourer.PourParticles?.Stop();
        pourer.DirtPourSound?.Stop();
        pourer.isDispensing = false;
    }
}

[HarmonyPatch(typeof(SoilPourer), "FillHovered", new Type[] { })]
internal static class SoilPourerManualFillHoveredPatch
{
    [HarmonyPostfix]
    private static void Postfix(SoilPourer __instance)
    {
        if (!RuntimeSettings.SoilPourerEnabled ||
            !RuntimeSettings.AllowManualMultiBagFill)
        {
            return;
        }

        int charges = SoilPourerChargeStore.GetCharges(__instance);
        if (charges > 0 && charges < RuntimeSettings.SoilPourerCapacity)
        {
            SoilPourerChargeStore.RefreshManualFillInteraction(__instance);
        }
    }
}

[HarmonyPatch(typeof(SoilPourer), "FillInteracted", new Type[] { })]
internal static class SoilPourerManualFillInteractedPatch
{
    [HarmonyPrefix]
    private static bool Prefix(SoilPourer __instance)
    {
        if (!RuntimeSettings.SoilPourerEnabled)
        {
            return true;
        }

        int charges = SoilPourerChargeStore.GetCharges(__instance);
        bool partialCustomFill = RuntimeSettings.AllowManualMultiBagFill &&
            charges > 0 &&
            charges < RuntimeSettings.SoilPourerCapacity;

        if (!partialCustomFill)
        {
            return true;
        }

        if (!SoilPourerAutomation.TryGetManualTopUp(
                __instance,
                out PlayerInventory? inventory,
                out ItemInstance? soilItem))
        {
            // Never hand a partly filled custom pourer to the native one-bag fill method.
            return false;
        }

        __instance.FillSound?.Play();
        SoilPourerChargeStore.RequestAddCharge(__instance, soilItem!.ID);
        inventory!.equippedSlot.ChangeQuantity(-1, false);
        RuntimeSettings.Debug(
            $"Player added {soilItem.ID} to soil pourer at {__instance._originCoordinate}; " +
            $"charge(s): {SoilPourerChargeStore.GetCharges(__instance)}/{RuntimeSettings.SoilPourerCapacity}.");
        return false;
    }
}

[HarmonyPatch(
    typeof(SoilPourer),
    "RpcLogic___SetSoil_2971853958",
    new[] { typeof(NetworkConnection), typeof(string) })]
internal static class SoilPourerNativeStatePatch
{
    [HarmonyPrefix]
    private static bool Prefix(SoilPourer __instance, string ID) =>
        !SoilPourerChargeStore.TryHandleNetworkMessage(__instance, ID);

    [HarmonyPostfix]
    private static void Postfix(SoilPourer __instance, string ID)
    {
        if (!SoilPourerChargeStore.IsNetworkMessage(ID))
        {
            SoilPourerChargeStore.ObserveNativeSoilChange(__instance, ID);
            if (InstanceFinder.IsServer)
            {
                SoilPourerChargeStore.BroadcastCharges(
                    __instance,
                    SoilPourerChargeStore.GetCharges(__instance));
            }
        }
    }
}

[HarmonyPatch(
    typeof(SoilPourer),
    "OnSpawnServer",
    new[] { typeof(NetworkConnection) })]
internal static class SoilPourerInitialChargeSyncPatch
{
    [HarmonyPostfix]
    private static void Postfix(SoilPourer __instance, NetworkConnection connection)
    {
        SoilPourerChargeStore.SendChargesToConnection(__instance, connection);
    }
}

[HarmonyPatch(typeof(SoilPourer), "HandleInteracted", new Type[] { })]
internal static class SoilPourerHandleInteractedPatch
{
    [HarmonyPrefix]
    private static bool Prefix(SoilPourer __instance)
    {
        if (!RuntimeSettings.SoilPourerEnabled ||
            !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryStartMultiChargeHandleDispense(__instance))
        {
            return true;
        }

        return false;
    }
}

[HarmonyPatch(typeof(SoilPourer), "RpcLogic___SendPourSoil_2166136261", new Type[] { })]
internal static class SoilPourerClientDispenseRequestPatch
{
    [HarmonyPrefix]
    private static bool Prefix(SoilPourer __instance)
    {
        if (!RuntimeSettings.SoilPourerEnabled ||
            !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryStartMultiChargeHandleDispense(__instance))
        {
            return true;
        }

        return false;
    }
}

[HarmonyPatch(typeof(SoilPourer), "RpcLogic___PourSoil_2166136261", new Type[] { })]
internal static class SoilPourerClientNativeDispenseSuppressionPatch
{
    [HarmonyPrefix]
    private static bool Prefix(SoilPourer __instance)
    {
        // A remote player's local SendPourSoil call immediately invokes the native observer
        // logic too. Suppress that one-bag coroutine; the server sends our charge-aware visual.
        return !RuntimeSettings.SoilPourerEnabled ||
            InstanceFinder.IsServer ||
            RuntimeSettings.SoilPourerCapacity <= 1 ||
            SoilPourerChargeStore.GetCharges(__instance) <= 0;
    }
}

[HarmonyPatch(typeof(Botanist), "UpdateBehaviour", new Type[] { })]
internal static class BotanistIdleSoilPourerRefillPatch
{
    [HarmonyPostfix]
    private static void Postfix(Botanist __instance)
    {
        if (RuntimeSettings.SoilPourerEnabled &&
            RuntimeSettings.AutoRefillSoilPourers &&
            InstanceFinder.IsServer)
        {
            SoilPourerAutomation.TryAssignIdleRefill(__instance);
        }
    }
}

[HarmonyPatch(typeof(GrowContainerBehaviour), "OnActiveTick", new Type[] { })]
internal static class SoilPourerRefillNavigationPatch
{
    private const float ArrivalDistance = 1.65f;
    private const float DestinationSuccessThreshold = 1.25f;
    private const float RetryDelaySeconds = 0.5f;

    private static readonly Dictionary<IntPtr, RefillWalkState> ActiveRefillWalks = new();

    [HarmonyPrefix]
    private static bool Prefix(GrowContainerBehaviour __instance)
    {
        AddSoilToGrowContainerBehaviour? behaviour =
            __instance.TryCast<AddSoilToGrowContainerBehaviour>();

        if (behaviour is null ||
            !RuntimeSettings.SoilPourerEnabled ||
            !RuntimeSettings.AutoRefillSoilPourers ||
            !InstanceFinder.IsServer ||
            behaviour._currentState != GrowContainerBehaviour.EState.Idle ||
            !SoilPourerAutomation.TryGetContext(
                behaviour,
                out GrowContainer? container,
                out _,
                out SoilPourer? pourer) ||
            !SoilPourerAutomation.ShouldRefill(container!, pourer!) ||
            !behaviour.IsRequiredItemInInventory(container!))
        {
            ActiveRefillWalks.Remove(__instance.Pointer);
            return true;
        }

        var npc = behaviour.Npc;
        var movement = npc?.Movement;
        if (npc is null || movement is null)
        {
            ActiveRefillWalks.Remove(__instance.Pointer);
            return true;
        }

        Vector3 destination = pourer!.transform.position;
        Vector3 botanistPosition = npc.transform.position;
        float deltaX = botanistPosition.x - destination.x;
        float deltaZ = botanistPosition.z - destination.z;
        bool isAtPourer = deltaX * deltaX + deltaZ * deltaZ <= ArrivalDistance * ArrivalDistance;

        if (isAtPourer && !movement.IsMoving)
        {
            ActiveRefillWalks.Remove(__instance.Pointer);
            RuntimeSettings.Debug(
                $"Botanist reached soil pourer at {pourer._originCoordinate}; starting refill action.");

            behaviour.PerformAction();
            movement.FacePoint(destination, 0.5f);
            return false;
        }

        float now = UnityEngine.Time.realtimeSinceStartup;
        bool needsDestination = !ActiveRefillWalks.TryGetValue(
                __instance.Pointer,
                out RefillWalkState state) ||
            state.PourerPointer != pourer.Pointer ||
            (!movement.IsMoving && now >= state.NextRetryTime);

        if (needsDestination)
        {
            behaviour.SetDestination(destination, true, DestinationSuccessThreshold);
            ActiveRefillWalks[__instance.Pointer] = new RefillWalkState(
                pourer.Pointer,
                now + RetryDelaySeconds);

            RuntimeSettings.Debug(
                $"Redirected soil-pourer refill walk to pourer at {pourer._originCoordinate}.");
        }

        // The native state checks arrival against the attached pot. While this is specifically
        // a dirt-carrying top-up trip, own the walk until the botanist reaches the pourer.
        return false;
    }

    private readonly record struct RefillWalkState(IntPtr PourerPointer, float NextRetryTime);
}

[HarmonyPatch(
    typeof(AddSoilToGrowContainerBehaviour),
    "AreTaskConditionsMetForContainer",
    new[] { typeof(GrowContainer) })]
internal static class SoilPourerRefillTaskConditionsPatch
{
    [HarmonyPostfix]
    private static void Postfix(
        AddSoilToGrowContainerBehaviour __instance,
        GrowContainer container,
        ref bool __result)
    {
        if (__result ||
            !RuntimeSettings.SoilPourerEnabled ||
            !RuntimeSettings.AutoRefillSoilPourers ||
            !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryGetContext(container, out _, out SoilPourer? pourer) ||
            !SoilPourerAutomation.ShouldRefill(container, pourer!))
        {
            return;
        }

        __result = container.PlayerUserObject is null &&
            (container.NPCUserObject is null ||
                container.NPCUserObject == __instance._botanist.NetworkObject) &&
            __instance.DoesBotanistHaveAccessToRequiredSupplies(container);
    }
}

[HarmonyPatch(
    typeof(AddSoilToGrowContainerBehaviour),
    "GetRequiredItemSuitableIDs",
    new[] { typeof(GrowContainer) })]
internal static class SoilPourerRequiredItemPatch
{
    [HarmonyPrefix]
    private static bool Prefix(
        GrowContainer growContainer,
        ref Il2CppStringArray __result)
    {
        if (!RuntimeSettings.SoilPourerEnabled || !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryGetContext(growContainer, out Pot? pot, out SoilPourer? pourer))
        {
            // An unattached pot stays entirely on the game's native soil path.
            return true;
        }

        int charges = SoilPourerChargeStore.GetCharges(pourer!);
        if (SoilPourerAutomation.ShouldRefill(growContainer, pourer!))
        {
            if (charges > 0)
            {
                // A multi-charge pourer may only be topped up with its existing soil type.
                __result = SoilPourerAutomation.GetStoredSoilID(pourer!);
                return false;
            }

            return true;
        }

        if (charges <= 0)
        {
            return true;
        }

        // The pourer supplies the soil. Fetch the pot's selected seed on the same trip when one
        // is configured, so this action can hand the planted pot directly to sprinkler watering.
        __result = SoilPourerAutomation.GetSelectedSeedIDs(pot!)!;
        return false;
    }
}

[HarmonyPatch(typeof(AddSoilToGrowContainerBehaviour), "GetActionEquippable", new Type[] { })]
internal static class SoilPourerActionEquippablePatch
{
    [HarmonyPrefix]
    private static bool Prefix(
        AddSoilToGrowContainerBehaviour __instance,
        ref AvatarEquippable __result)
    {
        if (!RuntimeSettings.SoilPourerEnabled || !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryGetContext(
                __instance,
                out GrowContainer? container,
                out Pot? pot,
                out SoilPourer? pourer) ||
            SoilPourerAutomation.ShouldRefill(container!, pourer!) ||
            SoilPourerChargeStore.GetCharges(pourer!) <= 0)
        {
            return true;
        }

        if (SoilPourerAutomation.GetSelectedSeedIDs(pot!) is not null)
        {
            return true;
        }

        __result = null!;
        return false;
    }
}

[HarmonyPatch(typeof(AddSoilToGrowContainerBehaviour), "GetAnimationBool", new Type[] { })]
internal static class SoilPourerAnimationPatch
{
    [HarmonyPrefix]
    private static bool Prefix(
        AddSoilToGrowContainerBehaviour __instance,
        ref string __result)
    {
        if (!RuntimeSettings.SoilPourerEnabled || !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryGetContext(
                __instance,
                out GrowContainer? container,
                out Pot? pot,
                out SoilPourer? pourer) ||
            SoilPourerAutomation.ShouldRefill(container!, pourer!) ||
            SoilPourerChargeStore.GetCharges(pourer!) <= 0)
        {
            return true;
        }

        __result = SoilPourerAutomation.GetSelectedSeedIDs(pot!) is null
            ? string.Empty
            : "PatSoil";
        return false;
    }
}

[HarmonyPatch(typeof(AddSoilToGrowContainerBehaviour), "GetActionDuration", new Type[] { })]
internal static class SoilPourerActionDurationPatch
{
    [HarmonyPostfix]
    private static void Postfix(AddSoilToGrowContainerBehaviour __instance, ref float __result)
    {
        if (RuntimeSettings.SoilPourerEnabled &&
            InstanceFinder.IsServer &&
            SoilPourerAutomation.TryGetContext(
                __instance,
                out GrowContainer? container,
                out _,
                out SoilPourer? pourer) &&
            !SoilPourerAutomation.ShouldRefill(container!, pourer!) &&
            SoilPourerChargeStore.GetCharges(pourer!) > 0)
        {
            __result = 0f;
        }
    }
}

[HarmonyPatch(
    typeof(AddSoilToGrowContainerBehaviour),
    "OnActionSuccess",
    new[] { typeof(ItemInstance) })]
internal static class SoilPourerActionSuccessPatch
{
    [HarmonyPrefix]
    private static bool Prefix(AddSoilToGrowContainerBehaviour __instance, ItemInstance usedItem)
    {
        if (!RuntimeSettings.SoilPourerEnabled || !InstanceFinder.IsServer ||
            !SoilPourerAutomation.TryGetContext(
                __instance,
                out GrowContainer? container,
                out Pot? pot,
                out SoilPourer? pourer))
        {
            return true;
        }

        SoilDefinition? carriedSoil = usedItem?.Definition?.TryCast<SoilDefinition>();
        if (carriedSoil is not null && SoilPourerAutomation.ShouldRefill(container!, pourer!))
        {
            SoilPourerChargeStore.AddCharge(pourer!, usedItem!.ID);
            SoilPourerAutomation.MarkRefillCompleted(__instance._botanist);
            RuntimeSettings.Debug(
                $"Botanist added {usedItem.ID} to soil pourer at {pourer!._originCoordinate}; " +
                $"charge(s): {SoilPourerChargeStore.GetCharges(pourer)}/{RuntimeSettings.SoilPourerCapacity}.");

            // The native coroutine consumes the carried bag and exits normally.
            return false;
        }

        bool usingStoredSoil = SoilPourerChargeStore.GetCharges(pourer!) > 0;
        if (!usingStoredSoil || !SoilPourerAutomation.TryApplySoil(container!, pourer!, usedItem))
        {
            return true;
        }

        if (usedItem is not null && usedItem.Definition?.TryCast<SeedDefinition>() is not null)
        {
            pot!.PlantSeed_Server(usedItem.ID, 0f);
            RuntimeSettings.Debug($"Planted {usedItem.ID} immediately after using the soil pourer.");
        }

        return false;
    }
}
#endif
