# Handover

## Current objective

Port the legacy Botanist Use Soil Pourer and Botanist Use Sprinkler IL2CPP mods to the current Schedule I grow-container API, while producing independent DLLs and one combined DLL with per-feature toggles.

## Target

- Schedule I `0.4.6f13`
- Unity `2022.3.62f2`
- MelonLoader `0.7.0 Open-Beta`
- Backend: IL2CPP

Reference assemblies are supplied externally using `MelonLoaderRoot`; none are committed.

## Compatibility evidence

The legacy DLLs fail because `Il2CppScheduleOne.NPCs.Behaviour.PotActionBehaviour` no longer exists. Their Harmony classes target the removed `StartAction`, `ActiveMinPass`, and `CompleteAction` methods.

The current API provides:

- `GrowContainerBehaviour`
- `AddSoilToGrowContainerBehaviour`
- `WaterPotBehaviour`
- `GrowContainerBehaviour.GetActionDuration()`
- `GrowContainerBehaviour.OnActionSuccess(ItemInstance)`
- `AddSoilToGrowContainerBehaviour.GetRequiredItemSuitableIDs(GrowContainer)`

The port patches only specialized soil and water behavior overrides. It leaves the base behavior's coroutine, item consumption, trash generation, and networked disable path intact.

## Verification record

Static API verification and builds are performed by `scripts/build-il2cpp.ps1`.

Verified on 2026-09-25 with:

```powershell
.\scripts\build-il2cpp.ps1 -MelonLoaderRoot '<game>\MelonLoader' -DotNet '<dotnet-6-sdk>\dotnet.exe'
```

Results:

- Game API verification: 27 checks passed.
- Build: succeeded with 0 warnings and 0 errors.
- Compiled artifact verification: 27 checks passed.
- `Assembly-CSharp.dll` SHA-256: `0D2EB364F3E84120AF7CCC9FA6BAFD597D42D495EBACC3A260CB4CA0CF0513DA`.
- `BotanistUseSoilPourer.dll` SHA-256: `1AB6316549C0E0F0E3FA64186205490BC28E50348E3AD464014239A9A94F2DFD`.
- `BotanistUseSprinkler.dll` SHA-256: `22CBCCC1A2EC57568EABFF2AF388FE4BB397721DD574052B87BA5A3722417877`.
- `BotanistsUseX.dll` SHA-256: `A07C59A07E68763644D53DBF6886C0E2B0018071CE0FFD7ED19FB3169C92535C`.
- `BotanistUseSoilPourer-2.0.0.zip` SHA-256: `61EB0A52E461DB8B4B5B963D01430353BBA5758221A5E6216FBE93CE096456C2`.
- `BotanistUseSprinkler-2.0.0.zip` SHA-256: `7F4CC52569876F089DC8EF98E45085BC221199B848131317DCE4C539FFE12AE9`.
- `BotanistsUseX-2.0.0.zip` SHA-256: `9FDEF1F4D37A93BAA7831EB900B54812D2D5BEDFDCCE7C14200A89C3E6DE8DAD`.

Runtime status: not yet tested in game. The pre-port failure is confirmed in the existing MelonLoader `Latest.log`; no game launch was required for diagnosis.

## Required runtime scenarios

### Soil Pourer

1. Attached pourer already contains each supported soil type.
2. Attached pourer is empty and the botanist fetches soil from supplies.
3. No attached pourer: normal botanist behavior remains unchanged.
4. Multiple pots attached to one pourer.
5. Host and remote-client visual/state synchronization.

### Sprinkler

1. Attached sprinkler has water and is ready.
2. Attached sprinkler is already sprinkling.
3. Attached sprinkler is empty or cooling down: normal hand watering remains available.
4. No attached sprinkler: normal botanist behavior remains unchanged.
5. Host and remote-client visual/state synchronization.

## Known limitations

- IL2CPP only; Mono/alternate is not claimed.
- Do not install the combined build with either individual build because that would apply duplicate Harmony patches.
- Runtime gameplay verification remains outstanding until the owner elects to launch and test the game.
