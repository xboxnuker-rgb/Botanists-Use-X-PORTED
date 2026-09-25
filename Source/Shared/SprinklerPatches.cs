#if FEATURE_SPRINKLER
using HarmonyLib;
using Il2CppFishNet;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.ObjectScripts;

namespace BotanistsUseX.Runtime;

internal static class SprinklerAutomation
{
    internal static Sprinkler? FindSprinkler(WaterPotBehaviour behaviour)
    {
        Pot? pot = behaviour._growContainer?.TryCast<Pot>();
        return pot is null ? null : DeviceLocator.FindUsableSprinkler(pot);
    }
}

[HarmonyPatch(typeof(WaterPotBehaviour), "GetActionDuration")]
internal static class SprinklerActionDurationPatch
{
    [HarmonyPostfix]
    private static void Postfix(WaterPotBehaviour __instance, ref float __result)
    {
        if (RuntimeSettings.SprinklerEnabled &&
            InstanceFinder.IsServer &&
            SprinklerAutomation.FindSprinkler(__instance) is not null)
        {
            __result = 0f;
        }
    }
}

[HarmonyPatch(typeof(WaterPotBehaviour), "OnActionSuccess")]
internal static class SprinklerActionSuccessPatch
{
    [HarmonyPrefix]
    private static bool Prefix(WaterPotBehaviour __instance)
    {
        if (!RuntimeSettings.SprinklerEnabled || !InstanceFinder.IsServer)
        {
            return true;
        }

        Sprinkler? sprinkler = SprinklerAutomation.FindSprinkler(__instance);
        if (sprinkler is null)
        {
            return true;
        }

        if (!sprinkler.IsSprinkling)
        {
            sprinkler.SendWater();
        }

        RuntimeSettings.Debug($"Used sprinkler at {sprinkler._originCoordinate}.");

        // The sprinkler owns the watering result. The game's coroutine still ends and disables
        // the botanist behavior through the normal GrowContainerBehaviour lifecycle.
        return false;
    }
}
#endif
