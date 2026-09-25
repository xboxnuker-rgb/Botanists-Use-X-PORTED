#if FEATURE_SOIL
using HarmonyLib;
using Il2CppFishNet;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.ObjectScripts;

namespace BotanistsUseX.Runtime;

internal static class SoilPourerAutomation
{
    internal static bool TryGetContext(
        AddSoilToGrowContainerBehaviour behaviour,
        out GrowContainer? container,
        out Pot? pot,
        out SoilPourer? pourer)
    {
        container = behaviour._growContainer;
        pot = container?.TryCast<Pot>();
        pourer = pot is null ? null : DeviceLocator.FindSoilPourer(pot);
        return container is not null && pot is not null && pourer is not null;
    }

    internal static bool TryApplySoil(
        GrowContainer container,
        SoilPourer pourer,
        ItemInstance? usedItem)
    {
        bool usingStoredSoil = !string.IsNullOrWhiteSpace(pourer.SoilID);
        SoilDefinition? soil = usingStoredSoil
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

        if (usingStoredSoil)
        {
            pourer.SendSoil(string.Empty);
            pourer.SetSoilLevel(0f);
        }

        RuntimeSettings.Debug($"Used soil pourer for grow container at {container._originCoordinate}.");
        return true;
    }
}

[HarmonyPatch(typeof(AddSoilToGrowContainerBehaviour), "GetRequiredItemSuitableIDs")]
internal static class SoilPourerRequiredItemPatch
{
    [HarmonyPrefix]
    private static bool Prefix(
        AddSoilToGrowContainerBehaviour __instance,
        ref Il2CppStringArray __result)
    {
        if (!RuntimeSettings.SoilPourerEnabled || !InstanceFinder.IsServer)
        {
            return true;
        }

        if (!SoilPourerAutomation.TryGetContext(__instance, out _, out _, out SoilPourer? pourer) ||
            string.IsNullOrWhiteSpace(pourer!.SoilID))
        {
            return true;
        }

        // A loaded pourer supplies the soil, so the botanist can walk directly to the pot.
        __result = null!;
        return false;
    }
}

[HarmonyPatch(typeof(AddSoilToGrowContainerBehaviour), "GetActionDuration")]
internal static class SoilPourerActionDurationPatch
{
    [HarmonyPostfix]
    private static void Postfix(AddSoilToGrowContainerBehaviour __instance, ref float __result)
    {
        if (RuntimeSettings.SoilPourerEnabled &&
            InstanceFinder.IsServer &&
            SoilPourerAutomation.TryGetContext(__instance, out _, out _, out _))
        {
            __result = 0f;
        }
    }
}

[HarmonyPatch(typeof(AddSoilToGrowContainerBehaviour), "OnActionSuccess")]
internal static class SoilPourerActionSuccessPatch
{
    [HarmonyPrefix]
    private static bool Prefix(AddSoilToGrowContainerBehaviour __instance, ItemInstance usedItem)
    {
        if (!RuntimeSettings.SoilPourerEnabled || !InstanceFinder.IsServer)
        {
            return true;
        }

        if (!SoilPourerAutomation.TryGetContext(
                __instance,
                out GrowContainer? container,
                out _,
                out SoilPourer? pourer))
        {
            return true;
        }

        // Returning false skips only the normal hand-pour result. The game's coroutine still
        // consumes a fetched soil item, creates its trash, and disables the behavior normally.
        return !SoilPourerAutomation.TryApplySoil(container!, pourer!, usedItem);
    }
}
#endif
