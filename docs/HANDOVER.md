# Handover

## Current objective

Port the legacy Botanist Use Soil Pourer and Botanist Use Sprinkler IL2CPP mods to the current Schedule I grow-container API, while producing independent DLLs and one combined DLL with per-feature toggles.

## Target

- Schedule I `0.4.7f12` (Steam public build `25796331`)
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

Reverified offline on 2026-10-10 with:

```powershell
.\scripts\build-il2cpp.ps1 -MelonLoaderRoot '<game>\MelonLoader' -DotNet '<dotnet-6-sdk>\dotnet.exe'
```

Results:

- Game API verification: 27 checks passed.
- Build: succeeded with 0 warnings and 0 errors.
- Compiled artifact verification: 36 checks passed, including startup-banner presence, invocation count, and per-build identity.
- `GameAssembly.dll` SHA-256: `61829050E56CA180D67BE592E4BFE61EC97D2E9573F720D90A54E318A7E4381D`.
- Generated `Assembly-CSharp.dll` SHA-256: `CEC1F3D02B38CEBB59A977FB8C23400094203C63FF7AAA5094143925475BB2CC`.
- `BotanistUseSoilPourer.dll` SHA-256: `8D68A1219DB28001B3F3CA09383FC2DC5D432A1F304C2250278B89FB0013BE2D`.
- `BotanistUseSprinkler.dll` SHA-256: `B48F958C530C59B08C06E0C551E83A625A69856FE07310796623E2CAB7DA66D2`.
- `BotanistsUseX.dll` SHA-256: `6C659029F20E2254D7FEB7AE38B1EEAAF5F0CEC31FF0CCDBC53CCC59DA7B25F3`.
- `BotanistUseSoilPourer-2.0.0.zip` SHA-256: `B5C30FBAFE97744429DBA2DD2A54F72448FA104A1EE65A6414158AE0CBBC6E4D`.
- `BotanistUseSprinkler-2.0.0.zip` SHA-256: `2CB09F2D2CE6704E67701CB6D13A61A05E48E46297ECD3E0645B2AA4D6CF19A4`.
- `BotanistsUseX-2.0.0.zip` SHA-256: `5AA63F9E57A73874CC2C108A61E7C643E00704201D6C20C58BCCAC1B8A039323`.

Cpp2IL inspection of the `0.4.7f12` native methods confirmed that the specialized soil and water success methods retain the expected semantics. The grow-container action coroutine still invokes the success method before its normal item decrement/trash path and ends through the networked behavior-disable path.

Runtime status: the rebuilt ports have not yet been enabled or tested in game. The pre-port failure is confirmed in an existing MelonLoader log; no game launch was performed for this revalidation.

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
