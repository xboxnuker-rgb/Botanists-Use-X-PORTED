# Nexus upload sheet — Botanist Use Soil Pourer PORTED

This sheet is ready to copy into the Schedule I Nexus Mods upload form after the permission checkpoint in [`docs/NEXUS_UPLOAD_DRAFTS.md`](../NEXUS_UPLOAD_DRAFTS.md) is resolved.

## Main details

**Game**

Schedule I

**Mod name**

Botanist Use Soil Pourer PORTED - 0.4.7 IL2CPP

**One-line summary**

Updated for Schedule I 0.4.7f12: botanists use and refill configurable multi-bag soil pourers, bring seeds, and synchronize state in multiplayer.

**Version**

2.0.0

**Category**

Employees

**Suggested tags**

Gameplay, Bug Fixes, Quality of Life, Utilities for Players, AI-Generated Content

**Author display**

Fortis | Ported by GSVS UK ACM

## General-page field map

- **Mod Name:** `Botanist Use Soil Pourer PORTED - 0.4.7 IL2CPP`
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
- The same `BotanistUseSoilPourer.dll` version on every multiplayer participant

No Mod Manager dependency is claimed.

## Full description

Use [`soil-pourer-description.md`](soil-pourer-description.md).

### Rich-text formatting plan

- Format `BOTANIST USE SOIL POURER PORTED` as the largest heading and colour it blue.
- Put the green compatibility callout directly below it.
- Format the port identity/credit line as a quote and bold Fortis and GSVS UK ACM.
- Use heading level 2 for major sections and level 3 for troubleshooting.
- Insert `soil-pourer-half-full.png` as the hero image after the opening section.
- Place `soil-pourer-empty.png` beside the multi-bag explanation.
- Place `botanist-device-workflow.png` beside How it works.
- Place `mods-folder-location.png` below Installation.
- Use the site default body text with restrained blue/green accents and horizontal rules.

## Main file

**File title**

Botanist Use Soil Pourer PORTED 2.0.0 - Schedule I 0.4.7f12 IL2CPP

**Upload filename**

`BotanistUseSoilPourer-2.0.0.zip`

**SHA-256**

`DC078755E4FAADD3A4E8DDF6221A37C9F538C238C11416E37B05858FD185147B`

**File description**

```text
Main Soil Pourer port. Includes BotanistUseSoilPourer.dll, installation README, MIT licence and attribution notice. Requires MelonLoader 0.7.0 Open-Beta. Remove the original/older Soil Pourer DLL first. Do not install beside BotanistsUseX.dll. Multiplayer participants should use the same version.
```

**Changelog**

```text
2.0.0
- Ported Soil Pourer automation to Schedule I 0.4.7f12 IL2CPP.
- Added configurable persistent 1-8 bag capacity.
- Added manual and botanist top-ups, charge prompts and proportional visuals.
- Added one-charge staged dispensing and partial-state refill interaction.
- Added immediate selected-seed planting after stored-soil use.
- Added idle preparation, corrected refill navigation and normal fallback.
- Added host-authoritative multiplayer synchronization.
```

## Permissions and disclosure answers

- **Original work/assets:** Based on BotanistUseSoilPourers by Fortis / Fortis202. Link the original Nexus page and source repository.
- **Permission basis:** Do not claim an open-source licence for the original. Resolve the repository's permission checkpoint before publication.
- **Other-site upload permission:** The original Nexus page currently restricts modified uploads/reuploads.
- **AI disclosure:** Select **AI-Generated Content**; explain human-directed, reviewed, compiled, and play-tested AI-assisted development. No generated media is included.
- **Donation Points:** opt out unless/until permission and any arrangement are explicit.
- **Adult content:** no.
- **Paid content:** no.

## Images and captions

Upload in this order:

1. `docs/images/soil-pourer-half-full.png`
   **Caption:** The modded 1/2 charge prompt and proportional half-fill state.
2. `docs/images/soil-pourer-empty.png`
   **Caption:** Empty soil pourer ready for another bag.
3. `docs/images/botanist-device-workflow.png`
   **Caption:** Botanist working at a pot beside linked grow equipment.
4. `docs/images/botanist-equipment-room.png`
   **Caption:** Multiple linked pourers prepared for the grow room.
5. `docs/images/mods-folder-location.png`
   **Caption:** Install BotanistUseSoilPourer.dll in the Schedule I Mods folder.

Use `soil-pourer-half-full.png` as the current primary image. Replace it with owner-approved Soil Pourer header art if supplied.

## Recommended links

- Maintained source and issues: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED
- Original source: https://github.com/fortiiis/BotanistUseSoilPourers
- Original Nexus page: https://www.nexusmods.com/schedule1/mods/1091
- MelonLoader: https://melonwiki.xyz/#/

## Pre-publish checklist

- [ ] Resolve and record the Fortis/Nexus permission checkpoint.
- [ ] Keep `PORTED` in the title and original-author credit visible.
- [ ] Set category `Employees`, version `2.0.0`, and the suggested tags.
- [ ] Upload `BotanistUseSoilPourer-2.0.0.zip`, not a source archive.
- [ ] Confirm DLL, README, licence, and notice appear in the preview.
- [ ] Apply the rich-text and image plan.
- [ ] Confirm the SHA-256 matches `docs/HANDOVER.md`.
- [ ] Save and preview; publish only after explicit owner approval.
