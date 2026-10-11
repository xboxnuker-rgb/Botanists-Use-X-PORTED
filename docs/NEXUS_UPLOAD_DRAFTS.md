# Nexus Upload Drafts

These drafts are ready for copy editing and image insertion. They have not been published. Version `2.0.0` targets Schedule I `0.4.7f12` IL2CPP with MelonLoader `0.7.0 Open-Beta`.

## Shared release notes

### Requirements

- Schedule I `0.4.7f12` on the IL2CPP branch
- MelonLoader `0.7.0 Open-Beta`
- The same mod DLL and version on every multiplayer participant

### Installation

1. Install MelonLoader for Schedule I.
2. Remove the old Fortis DLL for the feature being installed.
3. Copy the chosen DLL into the game's `Mods` folder.
4. Do not install `BotanistsUseX.dll` alongside either individual DLL.

Configuration files are generated under `UserData` on first launch. To uninstall, remove the DLL from `Mods`; saved configuration and soil-pourer charge data may be removed separately if a completely clean uninstall is wanted.

### Support request template

Please include the exact game version, mod version, whether you were host or client, the installed Botanists Use X DLL layout, reproduction steps, and the relevant MelonLoader log. Do not mix the combined build with an individual build.

---

## Listing 1 — Botanist Use Soil Pourer PORTED

### Suggested title

`Botanist Use Soil Pourer - IL2CPP Port`

### One-line summary

Botanists use, refill, and prepare multi-bag soil pourers on Schedule I 0.4.7f12, with synchronized multiplayer support.

### Main file

`BotanistUseSoilPourer-2.0.0.zip`

SHA-256: `40FF53B1933359F41D16F66F514D526BB0DF580A74914FDCF58FC6CB7E91FA78`

### Suggested description

Botanist Use Soil Pourer is back on the current Schedule I IL2CPP build.

This clean port replaces the removed legacy botanist action patches with the current grow-container behavior system. Botanists can use linked soil pourers, arrive with the configured seed, dispense soil, plant immediately, and refill pourers during idle work so the next planting cycle is prepared.

The port also expands soil pourers beyond the original one-bag behavior. Capacity is configurable from 1 to 8 bags and defaults to 2. Players and botanists can top up a partly filled pourer with matching soil, its visible fill level and prompt show the stored amount, and each dispense consumes one charge instead of emptying the whole device.

Features:

- Current Schedule I 0.4.7f12 IL2CPP compatibility
- Configurable 1–8 bag capacity
- Manual and botanist multi-bag refilling
- One-charge-at-a-time dispensing
- Proportional fill visuals and charge prompts
- Botanists bring the selected seed and plant immediately
- Refill-only trips navigate to the pourer
- Persistent charge state
- Host-authoritative multiplayer synchronization, including existing state for joining clients
- Blue GSVS `PORTED BY` startup banner

Multiplayer note: every participant should install the same DLL and version. The host controls capacity and charge state for the session.

Original mod concept and legacy implementation by Fortis. Port maintained by GSVS UK ACM / xboxnuker-rgb.

Source: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED

### Suggested tags

`Gameplay`, `Quality of Life`, `Utilities`, `Multiplayer`, `MelonLoader`

---

## Listing 2 — Botanist Use Sprinkler PORTED

### Suggested title

`Botanist Use Sprinkler - IL2CPP Port`

### One-line summary

Botanists activate linked small and large sprinklers instead of standing at pots and hand watering them.

### Main file

`BotanistUseSprinkler-2.0.0.zip`

SHA-256: `3C1E7F374874153AF3F883F1EC44A8AB469DAE1E0F341114733866D98E66EB83`

### Suggested description

Botanist Use Sprinkler is back on the current Schedule I IL2CPP build.

This clean port replaces the removed legacy botanist action patches with the current grow-container behavior system. When a linked sprinkler is ready, the botanist activates it instead of applying the normal hand-watering result. Both small and large sprinklers are supported, and the worker walks away as soon as the device is activated instead of remaining at the pot with a watering-can animation.

Features:

- Current Schedule I 0.4.7f12 IL2CPP compatibility
- Small and large sprinkler support
- Uses each sprinkler's authoritative linked-pot list
- Immediate worker release after activation
- Duplicate watering-task suppression while a sprinkler is running
- Normal hand-watering fallback when no linked sprinkler is ready
- Server-authoritative gameplay behavior
- Blue GSVS `PORTED BY` startup banner

For consistent multiplayer support, every participant should use the same DLL and version.

Original mod concept and legacy implementation by Fortis. Port maintained by GSVS UK ACM / xboxnuker-rgb.

Source: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED

### Suggested tags

`Gameplay`, `Quality of Life`, `Utilities`, `MelonLoader`

---

## Listing 3 — Botanists Use X

### Suggested title

`Botanists Use X - Soil Pourers and Sprinklers`

### One-line summary

One configurable DLL for botanist soil-pourer and sprinkler automation, including multi-bag pourers and multiplayer synchronization.

### Main file

`BotanistsUseX-2.0.0.zip`

SHA-256: `A6EF1669B265C1E57A5E39D0ED7CDF5C152FA6CEC8C7A7FDB119684EC9D4A418`

### Suggested description

Botanists Use X combines the modern Botanist Use Soil Pourer and Botanist Use Sprinkler ports in one configurable DLL.

Enable either feature or both. Botanists can prepare and use multi-bag soil pourers, bring the configured seed and plant it immediately, refill pourers during idle work, and activate linked small or large sprinklers before walking away to their next task.

Soil-pourer capacity is configurable from 1 to 8 bags and defaults to 2. Stored charges, prompts, fill visuals, remote interactions, staged dispensing, and joining-client state are synchronized from the host in multiplayer.

Features:

- Independent soil-pourer and sprinkler toggles
- Configurable automatic pourer refill
- Configurable 1–8 bag pourer capacity
- Optional manual multi-bag filling
- One-charge-at-a-time dispensing and proportional visuals
- Immediate soil-to-seed planting workflow
- Small and large sprinkler support
- Immediate worker release after sprinkler activation
- Host-authoritative multiplayer state synchronization
- Blue GSVS `CREATED BY` startup banner

Important: install this combined DLL by itself. Do not install it alongside either individual Botanist Use DLL. Every multiplayer participant should use the same DLL and version.

Based on the original Botanist Use Soil Pourer and Botanist Use Sprinkler concepts and legacy implementations by Fortis. Combined mod created and maintained by GSVS UK ACM / xboxnuker-rgb.

Source: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED

### Suggested tags

`Gameplay`, `Quality of Life`, `Utilities`, `Multiplayer`, `MelonLoader`

---

## Version 2.0.0 changelog for Nexus

- Ported both legacy mods to Schedule I 0.4.7f12 IL2CPP.
- Added the combined build with independent feature toggles.
- Added configurable multi-bag soil pourers, staged dispensing, manual/botanist top-ups, persistence, and multiplayer synchronization.
- Made botanists carry and immediately plant the configured seed after using a pourer.
- Added small and large sprinkler activation with immediate worker release.
- Added build/API verification and blue GSVS startup identity banners.

## Image assets — awaiting owner prompts

The owner supplied these unedited gameplay and installation screenshots, now stored under `docs/images`:

- `soil-pourer-empty.png` — empty pourer with the native `Insert soil` prompt.
- `soil-pourer-half-full.png` — the modded `Insert soil (1/2)` prompt and half-fill state.
- `botanist-equipment-room.png` — wider botanist and linked-equipment scene.
- `botanist-device-workflow.png` — close gameplay view of a botanist working beside linked equipment.
- `mods-folder-location.png` — Windows Explorer view of the Schedule I `Mods` path.

Recommended current use:

- Soil Pourer gallery: empty and half-full comparison.
- Combined-mod gallery: equipment-room and device-workflow shots.
- Shared installation section: Mods-folder screenshot.

Dedicated branded header/key art and a clearer sprinkler-specific action shot still await owner prompts:

1. Combined-mod Nexus header/key art.
2. Soil-pourer listing header/key art.
3. Sprinkler listing header/key art.
4. In-game full `2/2` pourer close-up, if a matching comparison shot is wanted.
5. In-game small/large sprinkler comparison or activation shot.
6. Optional blue GSVS startup-banner screenshot.

## Publication checklist

- Add owner-approved images and thumbnails.
- Confirm the selected Nexus category and permissions text.
- Attach the matching `2.0.0` archive to each listing.
- Recheck archive hashes against `docs/HANDOVER.md`.
- Publish only after explicit owner approval.
