# Changelog

All notable changes to this project are documented here.

## [2.0.0] - 2026-10-11

### Added

- Schedule I 0.4.7f12 IL2CPP ports for Botanist Use Soil Pourer and Botanist Use Sprinkler.
- A combined `BotanistsUseX.dll` build with independent soil-pourer and sprinkler toggles.
- Configurable automatic soil-pourer refilling, 1–8 bag capacity, and manual multi-bag filling.
- Persistent per-pourer charge counts, staged one-charge dispensing, matching fill prompts, and proportional soil-level visuals.
- Host-authoritative multiplayer synchronization for capacity, charge state, remote refills, dispensing, and clients joining an existing session.
- A shared blue-shaded `GSVS` startup banner with build-specific `PORTED BY`/`CREATED BY` wording and original-author credits.
- API and compiled-artifact verification scripts and reproducible builds using an external `MelonLoaderRoot`.
- GitHub release notes, three Nexus-ready description/upload sheets, supplied gameplay screenshots, and an explicit attribution notice.

### Changed

- Botanists carry the configured seed when using a loaded pourer and plant it immediately after soil is dispensed.
- Idle botanists top up eligible pourers for the next planting cycle.
- Refill trips target the pourer while the normal first planting trip continues to target the pot.
- Botanists walk away immediately after activating a small or large sprinkler instead of continuing a hand-watering animation.

### Fixed

- Port from the removed `PotActionBehaviour` API to the current grow-container behavior lifecycle.
- Discover linked soil pourers and sprinklers through each device's authoritative pot list instead of assuming the device origin lies on a cardinal tile near the pot.
- Avoid a null item-ID crash when a loaded soil pourer supplies the botanist's task.
- Prevent a still-dry pot from being reassigned while its linked sprinkler is already watering it.
- Avoid scene-wide sprinkler discovery from the worker task-eligibility path during save restoration.
- Let players add further matching soil bags to a partly filled pourer while preserving the native first-bag interaction.
- Prevent the native soil-pourer routine from emptying every configured charge in one dispense.
- Re-enable the fill interaction whenever a dispense or botanist action leaves a partial charge.
- Route remote-client top-up and handle requests through the same server-authoritative multi-bag logic used by the host.

### Verified

- Revalidated 70 exact Harmony targets and game API contracts against Schedule I 0.4.7f12.
- Verified 45 compiled-artifact requirements across all three DLLs.
- Runtime-tested manual and botanist soil-pourer workflows, staged dispensing, automatic refill, small and large sprinklers, and the combined build.
- Runtime-tested the final combined build in multiplayer, including host/client soil-pourer state and interaction synchronization.
