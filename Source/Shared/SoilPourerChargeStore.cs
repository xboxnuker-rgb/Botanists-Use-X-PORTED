#if FEATURE_SOIL
using System.Text.Json;
using Il2CppFishNet;
using Il2CppFishNet.Connection;
using MelonLoader;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.ObjectScripts;

namespace BotanistsUseX.Runtime;

internal static class SoilPourerChargeStore
{
    private const string SyncPrefix = "__GSVS_BUX_SYNC__:";
    private const string AddRequestPrefix = "__GSVS_BUX_ADD__:";
    private const string DispensePrefix = "__GSVS_BUX_DISPENSE__:";

    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };
    private static readonly Dictionary<string, int> ChargesByGuid = new(StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> VisualsInitialized = new(StringComparer.OrdinalIgnoreCase);
    private static string? _filePath;

    internal static void Initialize()
    {
        string directory = Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "BotanistsUseX");
        _filePath = Path.Combine(directory, "soil-pourer-charges.json");

        try
        {
            Directory.CreateDirectory(directory);
            if (!File.Exists(_filePath))
            {
                return;
            }

            Dictionary<string, int>? loaded = JsonSerializer.Deserialize<Dictionary<string, int>>(
                File.ReadAllText(_filePath),
                SerializerOptions);

            ChargesByGuid.Clear();
            VisualsInitialized.Clear();
            if (loaded is not null)
            {
                foreach ((string key, int value) in loaded)
                {
                    ChargesByGuid[key] = Math.Clamp(value, 0, RuntimeSettings.SoilPourerCapacity);
                }
            }
        }
        catch (Exception exception)
        {
            RuntimeSettings.Warning($"Could not load soil-pourer charge state: {exception.Message}");
        }
    }

    internal static int GetCharges(SoilPourer pourer)
    {
        string key = GetKey(pourer);

        if (string.IsNullOrWhiteSpace(pourer.SoilID))
        {
            ChargesByGuid[key] = 0;
            return 0;
        }

        if (!ChargesByGuid.TryGetValue(key, out int charges))
        {
            charges = 1;
            ChargesByGuid[key] = charges;
        }

        charges = Math.Clamp(charges, 1, RuntimeSettings.SoilPourerCapacity);
        if (VisualsInitialized.Add(key))
        {
            pourer.SetSoilLevel(charges / (float)RuntimeSettings.SoilPourerCapacity);
        }

        RefreshManualFillInteraction(pourer, charges);

        return charges;
    }

    internal static bool IsFull(SoilPourer pourer) =>
        GetCharges(pourer) >= RuntimeSettings.SoilPourerCapacity;

    internal static void AddCharge(SoilPourer pourer, string soilId)
    {
        int charges = Math.Min(GetCharges(pourer) + 1, RuntimeSettings.SoilPourerCapacity);
        if (string.IsNullOrWhiteSpace(pourer.SoilID))
        {
            // GetCharges returns zero for an empty pourer, so the first real bag establishes its soil type.
            charges = 1;
            pourer.SendSoil(soilId);
        }

        ChargesByGuid[GetKey(pourer)] = charges;
        VisualsInitialized.Add(GetKey(pourer));
        pourer.SetSoilLevel(charges / (float)RuntimeSettings.SoilPourerCapacity);
        RefreshManualFillInteraction(pourer, charges);
        Save();
        BroadcastCharges(pourer, charges);
    }

    internal static void RequestAddCharge(SoilPourer pourer, string soilId)
    {
        if (InstanceFinder.IsServer)
        {
            AddCharge(pourer, soilId);
            return;
        }

        pourer.SendSoil(AddRequestPrefix + soilId);
    }

    internal static int ConsumeCharge(SoilPourer pourer)
    {
        int charges = Math.Max(0, GetCharges(pourer) - 1);
        ChargesByGuid[GetKey(pourer)] = charges;
        VisualsInitialized.Add(GetKey(pourer));

        if (charges == 0)
        {
            pourer.SendSoil(string.Empty);
        }
        else
        {
            pourer.SetSoilLevel(charges / (float)RuntimeSettings.SoilPourerCapacity);
            BroadcastCharges(pourer, charges);
        }

        RefreshManualFillInteraction(pourer, charges);
        Save();
        return charges;
    }

    internal static int ReserveChargeForCustomDispense(SoilPourer pourer)
    {
        int charges = GetCharges(pourer);
        if (charges <= 0 || string.IsNullOrWhiteSpace(pourer.SoilID))
        {
            return -1;
        }

        string key = GetKey(pourer);
        int remainingCharges = charges - 1;
        ChargesByGuid[key] = remainingCharges;
        VisualsInitialized.Add(key);
        Save();
        return remainingCharges;
    }

    internal static void BroadcastCharges(SoilPourer pourer, int charges)
    {
        if (!InstanceFinder.IsServer)
        {
            return;
        }

        pourer.SetSoil(
            null!,
            $"{SyncPrefix}{Math.Clamp(charges, 0, RuntimeSettings.SoilPourerCapacity)}:" +
            RuntimeSettings.SoilPourerCapacity);
    }

    internal static void BroadcastDispenseStarted(
        SoilPourer pourer,
        int startingCharges,
        int remainingCharges)
    {
        if (!InstanceFinder.IsServer)
        {
            return;
        }

        pourer.SetSoil(
            null!,
            $"{DispensePrefix}{startingCharges}:{remainingCharges}:" +
            RuntimeSettings.SoilPourerCapacity);
    }

    internal static void SendChargesToConnection(SoilPourer pourer, NetworkConnection connection)
    {
        if (!InstanceFinder.IsServer || connection is null)
        {
            return;
        }

        int charges = GetCharges(pourer);
        pourer.SetSoil(
            connection,
            $"{SyncPrefix}{charges}:{RuntimeSettings.SoilPourerCapacity}");
    }

    internal static bool IsNetworkMessage(string value) =>
        value.StartsWith(SyncPrefix, StringComparison.Ordinal) ||
        value.StartsWith(AddRequestPrefix, StringComparison.Ordinal) ||
        value.StartsWith(DispensePrefix, StringComparison.Ordinal);

    internal static bool TryHandleNetworkMessage(SoilPourer pourer, string value)
    {
        if (value.StartsWith(SyncPrefix, StringComparison.Ordinal))
        {
            string[] parts = value[SyncPrefix.Length..].Split(':');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int charges) &&
                int.TryParse(parts[1], out int capacity) &&
                !InstanceFinder.IsServer)
            {
                RuntimeSettings.ApplyHostSoilPourerCapacity(capacity);
                ApplySyncedCharges(pourer, charges);
            }

            return true;
        }

        if (value.StartsWith(AddRequestPrefix, StringComparison.Ordinal))
        {
            if (InstanceFinder.IsServer)
            {
                string soilId = value[AddRequestPrefix.Length..];
                int charges = GetCharges(pourer);
                if (charges > 0 &&
                    charges < RuntimeSettings.SoilPourerCapacity &&
                    string.Equals(soilId, pourer.SoilID, StringComparison.OrdinalIgnoreCase))
                {
                    AddCharge(pourer, soilId);
                }
            }

            return true;
        }

        if (value.StartsWith(DispensePrefix, StringComparison.Ordinal))
        {
            string[] parts = value[DispensePrefix.Length..].Split(':');
            if (parts.Length == 3 &&
                int.TryParse(parts[0], out int startingCharges) &&
                int.TryParse(parts[1], out int remainingCharges) &&
                int.TryParse(parts[2], out int capacity) &&
                !InstanceFinder.IsServer)
            {
                RuntimeSettings.ApplyHostSoilPourerCapacity(capacity);
                SoilPourerAutomation.StartRemoteDispenseVisual(
                    pourer,
                    startingCharges,
                    remainingCharges);
            }

            return true;
        }

        return false;
    }

    internal static void RefreshManualFillInteraction(SoilPourer pourer)
    {
        RefreshManualFillInteraction(pourer, GetCharges(pourer));
    }

    internal static void ObserveNativeSoilChange(SoilPourer pourer, string soilId)
    {
        string key = GetKey(pourer);
        if (string.IsNullOrWhiteSpace(soilId))
        {
            ChargesByGuid[key] = 0;
            VisualsInitialized.Remove(key);
        }
        else if (!InstanceFinder.IsServer)
        {
            // The native payload represents one bag. A following mod sync message, when present,
            // replaces this with the host-authoritative multi-bag count.
            ChargesByGuid[key] = 1;
            VisualsInitialized.Add(key);
            pourer.SetSoilLevel(1f / RuntimeSettings.SoilPourerCapacity);
        }
        else if (!ChargesByGuid.TryGetValue(key, out int charges) || charges <= 0)
        {
            ChargesByGuid[key] = 1;
            VisualsInitialized.Add(key);
            pourer.SetSoilLevel(1f / RuntimeSettings.SoilPourerCapacity);
        }
        else
        {
            charges = Math.Clamp(charges, 1, RuntimeSettings.SoilPourerCapacity);
            VisualsInitialized.Add(key);
            pourer.SetSoilLevel(charges / (float)RuntimeSettings.SoilPourerCapacity);
        }

        if (!string.IsNullOrWhiteSpace(soilId))
        {
            RefreshManualFillInteraction(pourer, ChargesByGuid[key]);
        }

        Save();
    }

    private static void ApplySyncedCharges(SoilPourer pourer, int charges)
    {
        string key = GetKey(pourer);
        charges = Math.Clamp(charges, 0, RuntimeSettings.SoilPourerCapacity);
        ChargesByGuid[key] = charges;
        pourer.isDispensing = false;
        pourer.SetSoilLevel(charges / (float)RuntimeSettings.SoilPourerCapacity);

        if (charges > 0)
        {
            VisualsInitialized.Add(key);
            RefreshManualFillInteraction(pourer, charges);
        }
        else
        {
            VisualsInitialized.Remove(key);
        }
    }

    private static void RefreshManualFillInteraction(SoilPourer pourer, int charges)
    {
        if (!RuntimeSettings.AllowManualMultiBagFill || pourer.FillIntObj is null || charges <= 0)
        {
            return;
        }

        if (charges < RuntimeSettings.SoilPourerCapacity)
        {
            pourer.FillIntObj.SetMessage(
                $"Insert soil ({charges}/{RuntimeSettings.SoilPourerCapacity})");
            pourer.FillIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
        }
        else
        {
            pourer.FillIntObj.SetMessage(
                $"Soil pourer full ({charges}/{RuntimeSettings.SoilPourerCapacity})");
            pourer.FillIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
        }
    }

    private static string GetKey(SoilPourer pourer) => pourer.GUID.ToString();

    private static void Save()
    {
        if (!InstanceFinder.IsServer || string.IsNullOrWhiteSpace(_filePath))
        {
            return;
        }

        try
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(ChargesByGuid, SerializerOptions));
        }
        catch (Exception exception)
        {
            RuntimeSettings.Warning($"Could not save soil-pourer charge state: {exception.Message}");
        }
    }
}
#endif
