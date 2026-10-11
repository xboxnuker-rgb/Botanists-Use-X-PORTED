# Handover

## Release state

Version `2.0.0` is a runtime-verified release candidate containing:

- `BotanistUseSoilPourer.dll`
- `BotanistUseSprinkler.dll`
- `BotanistsUseX.dll`, with independent feature toggles

The individual builds and combined build share their gameplay implementation from `Source/Shared` and are behaviorally aligned. Do not install the combined DLL alongside either individual DLL.

## Target

- Schedule I `0.4.7f12` (Steam public build `25796331`)
- Unity `2022.3.62f2`
- MelonLoader `0.7.0 Open-Beta`
- Backend: IL2CPP

Reference assemblies are supplied externally using `MelonLoaderRoot`; none are committed.

## Root cause and compatibility work

The legacy DLLs targeted `Il2CppScheduleOne.NPCs.Behaviour.PotActionBehaviour`, which no longer exists. Their Harmony patches for `StartAction`, `ActiveMinPass`, and `CompleteAction` therefore failed during initialization.

The ports use the current grow-container behavior system, including `GrowContainerBehaviour`, `AddSoilToGrowContainerBehaviour`, and `WaterPotBehaviour`. Exact method signatures are verified before compilation. Specialized patches preserve the base behavior's normal item consumption, trash generation, coroutine lifecycle, and networked behavior shutdown unless a linked device owns the action.

## Implemented behavior

### Soil pourers

- Configurable 1–8 bag capacity, defaulting to 2.
- Persistent per-pourer charge counts and proportional fill visuals.
- Native first-bag fill plus optional manual matching-soil top-ups.
- One-charge staged dispensing, with the fill interaction restored after a partial dispense.
- Botanist idle-window refilling and navigation to the pourer on refill trips.
- Loaded-pourer trips fetch the configured seed and plant immediately after dispensing.
- Normal botanist soil behavior remains available when no eligible pourer is linked.

### Sprinklers

- Small and large linked-sprinkler discovery.
- Server-side activation in place of the hand-watering result.
- Immediate worker release after activation and duplicate-task suppression while watering.
- Normal hand watering remains available when no ready linked sprinkler exists.

### Multiplayer

- The host owns soil-pourer capacity and charge state.
- State, prompts, proportional visuals, refills, and staged dispense events are synchronized to clients through the game's existing FishNet RPC paths.
- Remote-client fill and handle requests run through the same authoritative logic as host interactions.
- Current state is sent when a client joins an existing session.
- Every participant should use the same DLL and version. The host capacity temporarily overrides a client's local capacity for that session.

## Verification record

The final source candidate was built and statically verified on 2026-10-11 with:

```powershell
.\scripts\build-il2cpp.ps1 -MelonLoaderRoot '<game>\MelonLoader' -DotNet '<dotnet-6-sdk>\dotnet.exe'
```

Results before final documentation packaging:

- Game API verification: 70 checks passed.
- Build: 0 warnings and 0 errors.
- Compiled artifact verification: 45 checks passed.
- `GameAssembly.dll` SHA-256: `61829050E56CA180D67BE592E4BFE61EC97D2E9573F720D90A54E318A7E4381D`.
- Generated `Assembly-CSharp.dll` SHA-256: `CEC1F3D02B38CEBB59A977FB8C23400094203C63FF7AAA5094143925475BB2CC`.
- `BotanistUseSoilPourer.dll` SHA-256: `15C451891FC56220ED543AEADBA1040AFF9ADEE71FDC635D57A0085F0B11E7B7`.
- `BotanistUseSprinkler.dll` SHA-256: `6D192555FEE0A7A515967546B079F3D73D4693B163CD320263FD3AC2DFECBF63`.
- `BotanistsUseX.dll` SHA-256: `619E685B538BCA1177A32ECD66201020931B12F3BEA8F1693FCBC3A083C0C99C`.

Final `2.0.0` archive hashes:

- `BotanistUseSoilPourer-2.0.0.zip` SHA-256: `40FF53B1933359F41D16F66F514D526BB0DF580A74914FDCF58FC6CB7E91FA78`.
- `BotanistUseSprinkler-2.0.0.zip` SHA-256: `3C1E7F374874153AF3F883F1EC44A8AB469DAE1E0F341114733866D98E66EB83`.
- `BotanistsUseX-2.0.0.zip` SHA-256: `A6EF1669B265C1E57A5E39D0ED7CDF5C152FA6CEC8C7A7FDB119684EC9D4A418`.

## Runtime verification

Runtime testing completed on 2026-10-11:

- Manual soil-pourer filling reaches 2/2 and the visual level and prompt track both charges.
- Manual dispensing consumes one charge per use: 2/2 → 1/2 → 0/2.
- A partly emptied pourer accepts another matching bag.
- Botanists use prefilled pourers, carry and immediately plant the configured seed, and do not stall after the action.
- Botanists refill eligible pourers during their idle cycle and walk to the pourer for refill-only trips.
- Pots without an eligible pourer retain normal dirt-bag behavior.
- Small and large sprinklers activate correctly and the botanist walks away after activation.
- The combined DLL matches the final individual builds and both feature toggles operate correctly.
- Multiplayer host/client testing passed with the final combined DLL: charge displays, two-bag behavior, refills, dispensing, and visual state remained synchronized.

## Final packages

Run:

```powershell
.\scripts\package.ps1 -MelonLoaderRoot '<game>\MelonLoader' -DotNet '<dotnet-6-sdk>\dotnet.exe'
```

Expected archives:

- `artifacts/BotanistUseSoilPourer-2.0.0.zip`
- `artifacts/BotanistUseSprinkler-2.0.0.zip`
- `artifacts/BotanistsUseX-2.0.0.zip`

The archives and build output are intentionally ignored by Git.

## Known limitations and support boundaries

- IL2CPP only; Mono/alternate is not claimed.
- Compatibility is claimed only for Schedule I `0.4.7f12` and the reference hashes above.
- Do not mix the combined build with either individual build because duplicate Harmony patches would be applied.
- Multiplayer support assumes every participant uses the same mod DLL and version.
- A later game update requires rerunning exact API, artifact, and runtime verification before compatibility is claimed.

## Release preparation

- Nexus copy for all three variants is drafted in `docs/NEXUS_UPLOAD_DRAFTS.md`.
- Image slots are deliberately left pending for owner-provided prompts.
- No Nexus page, GitHub release, merge, or public binary release should be created without separate owner approval.
