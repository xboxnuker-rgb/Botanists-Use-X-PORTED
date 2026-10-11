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
            debugEnabled: configuration.Debug,
            autoRefillSoilPourers: configuration.AutoRefillSoilPourers,
            soilPourerCapacity: configuration.SoilPourerCapacity,
            allowManualMultiBagFill: configuration.AllowManualMultiBagFill);

        SoilPourerChargeStore.Initialize();

        StartupBanner.Print("Botanist Use Soil Pourer", "Soil Pourer", isPortedBuild: true);

        RuntimeSettings.Info(
            $"Initialized. Enabled: {configuration.Enabled}; automatic refill: {configuration.AutoRefillSoilPourers}; " +
            $"capacity: {RuntimeSettings.SoilPourerCapacity}; manual multi-bag fill: {configuration.AllowManualMultiBagFill}; " +
            $"Debug: {configuration.Debug}.");
    }
}
internal sealed class Configuration
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("autoRefillSoilPourers")]
    public bool AutoRefillSoilPourers { get; set; } = true;

    [JsonPropertyName("soilPourerCapacity")]
    public int SoilPourerCapacity { get; set; } = 2;

    [JsonPropertyName("allowManualMultiBagFill")]
    public bool AllowManualMultiBagFill { get; set; } = true;

    [JsonPropertyName("debug")]
    public bool Debug { get; set; }
}
