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

Reverified offline on 2026-10-09 with:

```powershell
.\scripts\build-il2cpp.ps1 -MelonLoaderRoot '<game>\MelonLoader' -DotNet '<dotnet-6-sdk>\dotnet.exe'
```

Results:

- Game API verification: 27 checks passed.
- Build: succeeded with 0 warnings and 0 errors.
- Compiled artifact verification: 27 checks passed.
- `GameAssembly.dll` SHA-256: `61829050E56CA180D67BE592E4BFE61EC97D2E9573F720D90A54E318A7E4381D`.
- Generated `Assembly-CSharp.dll` SHA-256: `CEC1F3D02B38CEBB59A977FB8C23400094203C63FF7AAA5094143925475BB2CC`.
- `BotanistUseSoilPourer.dll` SHA-256: `BFAD9337787B667071A8CB65B15A17652EFDB78759C54418B65115EADFC52B76`.
- `BotanistUseSprinkler.dll` SHA-256: `7D236EB0EA2D1734A513644E7A9CDEE89D9291768D92C050A29E8D5529CFBC68`.
- `BotanistsUseX.dll` SHA-256: `5B430BF64846B519485B5F58A39071E537B698E28D50CF025BC1540A34B395F8`.
- `BotanistUseSoilPourer-2.0.0.zip` SHA-256: `3B4820EF9AEE905FAAB9504BD1A4D00E07AE933ECD0DC56B4A3A6F80905DD829`.
- `BotanistUseSprinkler-2.0.0.zip` SHA-256: `3BF018D84C876339418957951AEDBBA00C9C1ED84910B74F8194D6B6211A73ED`.
- `BotanistsUseX-2.0.0.zip` SHA-256: `B3CB335AFD7F03186DB256FB952AA4E9DD002E99D01FF7DFC87562A7144C0A57`.

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
