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

The final source candidate was rebuilt, packaged, and statically verified on 2026-10-12 with:

```powershell
.\scripts\build-il2cpp.ps1 -MelonLoaderRoot '<game>\MelonLoader' -DotNet '<dotnet-6-sdk>\dotnet.exe'
```

Results before final documentation packaging:

- Game API verification: 70 checks passed.
- Build: 0 warnings and 0 errors.
- Compiled artifact verification: 45 checks passed.
- `GameAssembly.dll` SHA-256: `61829050E56CA180D67BE592E4BFE61EC97D2E9573F720D90A54E318A7E4381D`.
- Generated `Assembly-CSharp.dll` SHA-256: `CEC1F3D02B38CEBB59A977FB8C23400094203C63FF7AAA5094143925475BB2CC`.
- `BotanistUseSoilPourer.dll` SHA-256: `8C16889E1E205B34FA62012135627456588F30AFFA9A61676C5FCFD6B8F1C33C`.
- `BotanistUseSprinkler.dll` SHA-256: `293BD398E4CCAEF41BC1704AC7D5C9DA276F4AB8A2531F37F9000ECDE1962F4F`.
- `BotanistsUseX.dll` SHA-256: `2F417EA782C02586E537257EFA4A80730F01585E3AA23265A8663ECC7DC5D491`.

Final `2.0.0` archive hashes:

- `BotanistUseSoilPourer-2.0.0.zip` SHA-256: `DC078755E4FAADD3A4E8DDF6221A37C9F538C238C11416E37B05858FD185147B`.
- `BotanistUseSprinkler-2.0.0.zip` SHA-256: `2C2CB1F5A742D8BFDB446C4426636535EFE9105F0CA487867D41964167C1918D`.
- `BotanistsUseX-2.0.0.zip` SHA-256: `BB249451D15A1BF314A89671DEA8E3536406E444FE85B10946C30B026B73240F`.

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

Each archive contains its DLL, `README.md`, `LICENSE.txt`, and `NOTICE.txt`. Archives and build output are intentionally ignored by Git.

## Known limitations and support boundaries

- IL2CPP only; Mono/alternate is not claimed.
- Compatibility is claimed only for Schedule I `0.4.7f12` and the reference hashes above.
- Do not mix the combined build with either individual build because duplicate Harmony patches would be applied.
- Multiplayer support assumes every participant uses the same mod DLL and version.
- A later game update requires rerunning exact API, artifact, and runtime verification before compatibility is claimed.

## Release preparation

- GitHub release copy is drafted in `docs/RELEASE_NOTES_2.0.0.md`.
- Nexus descriptions and upload sheets for all three variants are indexed in `docs/NEXUS_UPLOAD_DRAFTS.md`.
- Supplied screenshots are committed under `docs/images`; branded headers and a dedicated sprinkler action shot remain pending.
- Nexus publication is blocked until the Fortis/Nexus permission checkpoint in the drafts is resolved.
- No Nexus page, GitHub release, merge, or public binary release should be created without separate owner approval.
