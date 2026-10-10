using MelonLoader;

namespace BotanistsUseX.Runtime;

internal static class StartupBanner
{
    internal const string ModVersion = "2.0.0";
    internal const string TargetGameVersion = "0.4.7f12";

    private const string Reset = "\u001b[0m";
    private const string White = "\u001b[97m";
    private const string Green = "\u001b[1;38;5;46m";

    // Different purple shades create the vertical shading.
    private const string Purple1 = "\u001b[1;38;5;141m";
    private const string Purple2 = "\u001b[1;38;5;135m";
    private const string Purple3 = "\u001b[38;5;99m";
    private const string Purple4 = "\u001b[38;5;93m";
    private const string Purple5 = "\u001b[38;5;57m";
    private const string Purple6 = "\u001b[38;5;54m";

    internal static void Print(string modName, string features)
    {
        MelonLogger.Msg(string.Empty);

        MelonLogger.Msg(
            $"{Purple6}──────────────{Reset} " +
            $"{Green}CREATED BY{Reset} " +
            $"{Purple6}──────────────{Reset}");

        PrintRow(Purple1, " ██████╗  ██╗   ██╗ ██╗  ██╗ ", "Mod: ", modName);
        PrintRow(Purple2, " ██╔══██╗ ██║   ██║ ╚██╗██╔╝ ", "Version: ", $"v{ModVersion}");
        PrintRow(Purple3, " ██████╔╝ ██║   ██║  ╚███╔╝  ", "Game: ", $"Schedule I {TargetGameVersion}");
        PrintRow(Purple4, " ██╔══██╗ ██║   ██║  ██╔██╗  ", "Backend: ", "IL2CPP");
        PrintRow(Purple5, " ██████╔╝ ╚██████╔╝ ██╔╝ ██╗ ", "Features: ", features);
        PrintRow(Purple6, " ╚═════╝   ╚═════╝  ╚═╝  ╚═╝ ", "Created by: ", "GSVS UK ACM / xboxnuker-rgb");

        MelonLogger.Msg(
            $"{Purple6}─────────────────{Reset} " +
            $"{White}Original concepts by {Reset}" +
            $"{Green}Fortis{Reset} " +
            $"{Purple6}─────────────────{Reset}");
    }

    private static void PrintRow(string artColor, string art, string label, string value)
    {
        MelonLogger.Msg(
            $"{artColor}{art}{Reset}  " +
            $"{White}{label}{Reset}" +
            $"{Green}{value}{Reset}");
    }
}
