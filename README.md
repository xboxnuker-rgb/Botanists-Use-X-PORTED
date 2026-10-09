# Botanists Use X — Schedule I IL2CPP Ports

Modern IL2CPP ports of two botanist equipment-automation mods, plus a combined configurable build:

- `BotanistUseSoilPourer.dll`
- `BotanistUseSprinkler.dll`
- `BotanistsUseX.dll`

These builds target Schedule I `0.4.7f12` IL2CPP and its replacement grow-container behavior API. The legacy mods patched the removed `PotActionBehaviour` type and consequently failed during Harmony initialization.

## Features

### Soil Pourer

- Detects a soil pourer attached to the botanist's assigned pot.
- Walks directly to the pot when the attached pourer already contains soil.
- Uses soil fetched by the botanist when the attached pourer is empty.
- Applies soil immediately through the current `GrowContainer` API.
- Preserves the game's normal item consumption, trash creation, and behavior shutdown.

### Sprinkler

- Detects a usable sprinkler attached to the assigned pot.
- Activates it instead of applying the botanist's normal hand-watering result.
- Completes through the current grow-container behavior lifecycle.
- Falls back to normal hand watering when no usable sprinkler exists.

Both features make gameplay changes only on the server/host.

## Choose one installation layout

Install either:

1. `BotanistsUseX.dll` for both features and independent toggles; or
2. Either or both individual DLLs.

Do not install the combined DLL alongside either individual DLL. Remove the legacy Fortis DLLs before installing these ports.

Copy the chosen DLLs into the game's `Mods` directory.

## Configuration

Configuration files are created on first launch under `UserData`.

Combined build (`UserData/BotanistsUseX/configuration.json`):

```json
{
  "soilPourerEnabled": true,
  "sprinklerEnabled": true,
  "debug": false
}
```

Individual builds use their existing-compatible directories:

- `UserData/BotanistUseSoilPourer/configuration.json`
- `UserData/BotanistUseSprinkler/configuration.json`

Each individual file has `enabled` and `debug` Boolean settings.

## Build

Requirements:

- Windows PowerShell 7 or Windows PowerShell 5.1
- .NET 6 SDK
- Schedule I `0.4.7f12` IL2CPP with MelonLoader `0.7.0`

Build and statically verify all three DLLs:

```powershell
.\scripts\build-il2cpp.ps1 `
  -MelonLoaderRoot 'C:\Games\Schedule I\MelonLoader'
```

The script validates exact game API contracts before compiling. Outputs are written to `bin/Il2Cpp` and are not committed.

## Compatibility status

- Schedule I: `0.4.7f12`
- Backend: IL2CPP
- Unity: `2022.3.62f2`
- MelonLoader: `0.7.0 Open-Beta`
- Mono/alternate branch: not currently supported

A successful build and static Harmony target check are not substitutes for an in-game host/client test. See [docs/HANDOVER.md](docs/HANDOVER.md) for the current verification record.

## Credits

The original Botanist Use Sprinkler and Botanist Use Soil Pourer concepts and legacy implementations were created by Fortis. This repository is a clean port to the replacement Schedule I grow-container behavior system.
