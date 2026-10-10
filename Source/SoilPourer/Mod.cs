using System.Text.Json.Serialization;
using BotanistsUseX.Runtime;
using MelonLoader;

[assembly: MelonInfo(typeof(BotanistUseSoilPourer.BotanistUseSoilPourerMod), "Botanist Use Soil Pourer (Ported)", StartupBanner.ModVersion, "xboxnuker-rgb; original concept by Fortis")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BotanistUseSoilPourer;

public sealed class BotanistUseSoilPourerMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        Configuration configuration = ConfigurationFile.Load(
            "BotanistUseSoilPourer",
            static () => new Configuration());

        RuntimeSettings.Configure(
            soilPourerEnabled: configuration.Enabled,
            sprinklerEnabled: false,
            debugEnabled: configuration.Debug);

        StartupBanner.Print("Botanist Use Soil Pourer", "Soil Pourer");

        RuntimeSettings.Info($"Initialized. Enabled: {configuration.Enabled}; Debug: {configuration.Debug}.");
    }
}
internal sealed class Configuration
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("debug")]
    public bool Debug { get; set; }
}
