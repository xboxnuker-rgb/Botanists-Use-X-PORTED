using System.Text.Json.Serialization;
using BotanistsUseX.Runtime;
using MelonLoader;

[assembly: MelonInfo(typeof(BotanistsUseX.BotanistsUseXMod), "Botanists Use X", StartupBanner.ModVersion, "xboxnuker-rgb; original concepts by Fortis")]
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
            configuration.Debug,
            configuration.AutoRefillSoilPourers,
            configuration.SoilPourerCapacity,
            configuration.AllowManualMultiBagFill);

        SoilPourerChargeStore.Initialize();

        StartupBanner.Print("Botanists Use X", "Soil Pourer + Sprinkler", isPortedBuild: false);

        RuntimeSettings.Info(
            $"Initialized. Soil pourers: {configuration.SoilPourerEnabled}; " +
            $"automatic refill: {configuration.AutoRefillSoilPourers}; " +
            $"capacity: {RuntimeSettings.SoilPourerCapacity}; " +
            $"manual multi-bag fill: {configuration.AllowManualMultiBagFill}; " +
            $"Sprinklers: {configuration.SprinklerEnabled}; Debug: {configuration.Debug}.");
    }
}
internal sealed class Configuration
{
    [JsonPropertyName("soilPourerEnabled")]
    public bool SoilPourerEnabled { get; set; } = true;

    [JsonPropertyName("autoRefillSoilPourers")]
    public bool AutoRefillSoilPourers { get; set; } = true;

    [JsonPropertyName("soilPourerCapacity")]
    public int SoilPourerCapacity { get; set; } = 2;

    [JsonPropertyName("allowManualMultiBagFill")]
    public bool AllowManualMultiBagFill { get; set; } = true;

    [JsonPropertyName("sprinklerEnabled")]
    public bool SprinklerEnabled { get; set; } = true;

    [JsonPropertyName("debug")]
    public bool Debug { get; set; }
}
