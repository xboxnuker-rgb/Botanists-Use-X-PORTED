using MelonLoader;

namespace BotanistsUseX.Runtime;

internal static class RuntimeSettings
{
    internal static bool SoilPourerEnabled { get; private set; } = true;

    internal static bool SprinklerEnabled { get; private set; } = true;

    internal static bool DebugEnabled { get; private set; }

    internal static void Configure(bool soilPourerEnabled, bool sprinklerEnabled, bool debugEnabled)
    {
        SoilPourerEnabled = soilPourerEnabled;
        SprinklerEnabled = sprinklerEnabled;
        DebugEnabled = debugEnabled;
    }

    internal static void Debug(string message)
    {
        if (DebugEnabled)
        {
            MelonLogger.Msg($"[DEBUG] {message}");
        }
    }

    internal static void Info(string message) => MelonLogger.Msg($"[INFO] {message}");

    internal static void Warning(string message) => MelonLogger.Warning(message);

    internal static void Error(string message) => MelonLogger.Error(message);
}
