using System.Text.Json.Serialization;
using BotanistsUseX.Runtime;
using MelonLoader;

[assembly: MelonInfo(typeof(BotanistUseSprinkler.BotanistUseSprinklerMod), "Botanist Use Sprinkler (Ported)", "2.0.0", "xboxnuker-rgb; original concept by Fortis")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BotanistUseSprinkler;

public sealed class BotanistUseSprinklerMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        Configuration configuration = ConfigurationFile.Load(
            "BotanistUseSprinkler",
            static () => new Configuration());

        RuntimeSettings.Configure(
            soilPourerEnabled: false,
            sprinklerEnabled: configuration.Enabled,
            debugEnabled: configuration.Debug);

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
