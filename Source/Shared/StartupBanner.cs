using MelonLoader;

namespace BotanistsUseX.Runtime;

internal static class StartupBanner
{
    internal const string ModVersion = "2.0.0";
    internal const string TargetGameVersion = "0.4.7f12";

    private const string Reset = "\u001b[0m";
    private const string White = "\u001b[97m";
    private const string BrightBlue = "\u001b[1;38;5;39m";

    // Different blue shades create the vertical shading.
    private const string Blue1 = "\u001b[1;38;5;81m";
    private const string Blue2 = "\u001b[1;38;5;75m";
    private const string Blue3 = "\u001b[38;5;69m";
    private const string Blue4 = "\u001b[38;5;63m";
    private const string Blue5 = "\u001b[38;5;33m";
    private const string Blue6 = "\u001b[38;5;27m";

    internal static void Print(string modName, string features, bool isPortedBuild)
    {
        string heading = isPortedBuild ? "PORTED BY" : "CREATED BY";
        string creatorLabel = isPortedBuild ? "Ported by: " : "Created by: ";
        string originalLabel = isPortedBuild ? "Original mod by " : "Original concepts by ";

        MelonLogger.Msg(string.Empty);

        MelonLogger.Msg(
            $"{Blue6}──────────────{Reset} " +
            $"{BrightBlue}{heading}{Reset} " +
            $"{Blue6}──────────────{Reset}");

        PrintRow(Blue1, "  ██████╗  ███████╗ ██╗   ██╗ ███████╗  ", "Mod: ", modName);
        PrintRow(Blue2, " ██╔════╝  ██╔════╝ ██║   ██║ ██╔════╝  ", "Version: ", $"v{ModVersion}");
        PrintRow(Blue3, " ██║  ███╗ ███████╗ ██║   ██║ ███████╗  ", "Game: ", $"Schedule I {TargetGameVersion}");
        PrintRow(Blue4, " ██║   ██║ ╚════██║ ╚██╗ ██╔╝ ╚════██║  ", "Backend: ", "IL2CPP");
        PrintRow(Blue5, " ╚██████╔╝ ███████║  ╚████╔╝  ███████║  ", "Features: ", features);
        PrintRow(Blue6, "  ╚═════╝  ╚══════╝   ╚═══╝   ╚══════╝  ", creatorLabel, "GSVS UK ACM / xboxnuker-rgb");

        MelonLogger.Msg(
            $"{Blue6}─────────────────{Reset} " +
            $"{White}{originalLabel}{Reset}" +
            $"{BrightBlue}Fortis{Reset} " +
            $"{Blue6}─────────────────{Reset}");
    }

    private static void PrintRow(string artColor, string art, string label, string value)
    {
        MelonLogger.Msg(
            $"{artColor}{art}{Reset}  " +
            $"{White}{label}{Reset}" +
            $"{BrightBlue}{value}{Reset}");
    }
}
