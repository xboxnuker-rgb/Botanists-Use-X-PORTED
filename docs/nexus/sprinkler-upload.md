# Nexus upload sheet — Botanist Use Sprinkler PORTED

This sheet is ready to copy into the Schedule I Nexus Mods upload form after the permission checkpoint in [`docs/NEXUS_UPLOAD_DRAFTS.md`](../NEXUS_UPLOAD_DRAFTS.md) is resolved.

## Main details

**Game**

Schedule I

**Mod name**

Botanist Use Sprinkler PORTED - 0.4.7 IL2CPP

**One-line summary**

Updated for Schedule I 0.4.7f12: botanists activate linked small and large sprinklers, skip hand watering, and move to their next task.

**Version**

2.0.0

**Category**

Employees

**Suggested tags**

Gameplay, Bug Fixes, Quality of Life, Utilities for Players, AI-Generated Content

**Author display**

Fortis | Ported by GSVS UK ACM

## General-page field map

- **Mod Name:** `Botanist Use Sprinkler PORTED - 0.4.7 IL2CPP`
- **Game:** `Schedule I`
- **Category:** `Employees`
- **Mod version:** `2.0.0`
- **Author or team name:** `Fortis | Ported by GSVS UK ACM`
- **Short description:** use the one-line summary above
- **Translation of another mod:** leave unchecked
- **Community tagging:** leave enabled
- **Download mirrors:** leave empty
- **Save/Publish:** save and preview only; do not publish until permission and owner approval are recorded

## Requirements

- Schedule I 0.4.7f12 IL2CPP/main branch
- MelonLoader 0.7.0 Open-Beta
- The same `BotanistUseSprinkler.dll` version on multiplayer participants

No Mod Manager dependency is claimed.

## Full description

Use [`sprinkler-description.md`](sprinkler-description.md).

### Rich-text formatting plan

- Format `BOTANIST USE SPRINKLER PORTED` as the largest heading and colour it blue.
- Put the green compatibility callout directly below it.
- Format the port identity/credit line as a quote and bold Fortis and GSVS UK ACM.
- Use heading level 2 for major sections and level 3 for troubleshooting.
- Use an owner-approved small/large sprinkler action image as the hero; this remains pending.
- Until that exists, place `botanist-equipment-room.png` after the opening section without using it as the final primary image.
- Place `botanist-device-workflow.png` near How it works.
- Place `mods-folder-location.png` below Installation.
- Use the site default body text with restrained blue/green accents and horizontal rules.

## Main file

**File title**

Botanist Use Sprinkler PORTED 2.0.0 - Schedule I 0.4.7f12 IL2CPP

**Upload filename**

`BotanistUseSprinkler-2.0.0.zip`

**SHA-256**

`2C2CB1F5A742D8BFDB446C4426636535EFE9105F0CA487867D41964167C1918D`

**File description**

```text
Main Sprinkler port. Includes BotanistUseSprinkler.dll, installation README, MIT licence and attribution notice. Requires MelonLoader 0.7.0 Open-Beta. Remove the original/older Sprinkler DLL first. Do not install beside BotanistsUseX.dll. Use the same version across multiplayer participants.
```

**Changelog**

```text
2.0.0
- Ported Sprinkler automation to Schedule I 0.4.7f12 IL2CPP.
- Added authoritative linked-device discovery for small and large sprinklers.
- Skipped redundant watering equipment and animation when the device owns the job.
- Released the botanist immediately after successful activation.
- Prevented duplicate watering work while a linked sprinkler is active.
- Preserved normal hand-watering fallback.
```

## Permissions and disclosure answers

- **Original work/assets:** Based on BotanistUseSprinkler by Fortis / Fortis202. Link the original Nexus page and source repository.
- **Permission basis:** Do not claim an open-source licence for the original. Resolve the repository's permission checkpoint before publication.
- **Other-site upload permission:** The original Nexus page currently restricts modified uploads/reuploads.
- **AI disclosure:** Select **AI-Generated Content**; explain human-directed, reviewed, compiled, and play-tested AI-assisted development. No generated media is included.
- **Donation Points:** opt out unless/until permission and any arrangement are explicit.
- **Adult content:** no.
- **Paid content:** no.

## Images and captions

Current supporting images:

1. `docs/images/botanist-equipment-room.png`
   **Caption:** Botanists working among linked grow equipment.
2. `docs/images/botanist-device-workflow.png`
   **Caption:** Botanist completing a grow task beside linked equipment.
3. `docs/images/mods-folder-location.png`
   **Caption:** Install BotanistUseSprinkler.dll in the Schedule I Mods folder.

Before publication, add a dedicated screenshot clearly showing small and/or large sprinkler activation and use it as the primary image. Branded header art also awaits the owner's prompt.

## Recommended links

- Maintained source and issues: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED
- Original source: https://github.com/fortiiis/BotanistUseSprinkler
- Original Nexus page: https://www.nexusmods.com/schedule1/mods/1080
- MelonLoader: https://melonwiki.xyz/#/

## Pre-publish checklist

- [ ] Resolve and record the Fortis/Nexus permission checkpoint.
- [ ] Add a dedicated sprinkler action image and choose it as primary.
- [ ] Keep `PORTED` in the title and original-author credit visible.
- [ ] Set category `Employees`, version `2.0.0`, and the suggested tags.
- [ ] Upload `BotanistUseSprinkler-2.0.0.zip`, not a source archive.
- [ ] Confirm DLL, README, licence, and notice appear in the preview.
- [ ] Apply the rich-text and image plan.
- [ ] Confirm the SHA-256 matches `docs/HANDOVER.md`.
- [ ] Save and preview; publish only after explicit owner approval.
