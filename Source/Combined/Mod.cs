using System.Text.Json.Serialization;
using BotanistsUseX.Runtime;
using MelonLoader;

[assembly: MelonInfo(typeof(BotanistsUseX.BotanistsUseXMod), "Botanists Use X", "2.0.0", "xboxnuker-rgb; original concepts by Fortis")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BotanistsUseX;

public sealed class BotanistsUseXMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        Configuration configuration = ConfigurationFile.Load(
            "BotanistsUseX",
            static () => new Configuration());

        RuntimeSettings.Configure(
            configuration.SoilPourerEnabled,
            configuration.SprinklerEnabled,
            configuration.Debug);

        RuntimeSettings.Info(
            $"Initialized. Soil pourers: {configuration.SoilPourerEnabled}; " +
            $"Sprinklers: {configuration.SprinklerEnabled}; Debug: {configuration.Debug}.");
    }
}
internal sealed class Configuration
{
    [JsonPropertyName("soilPourerEnabled")]
    public bool SoilPourerEnabled { get; set; } = true;

    [JsonPropertyName("sprinklerEnabled")]
    public bool SprinklerEnabled { get; set; } = true;

    [JsonPropertyName("debug")]
    public bool Debug { get; set; }
}
