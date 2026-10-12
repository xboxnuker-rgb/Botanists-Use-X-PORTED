# Nexus upload sheet — Botanists Use X

This sheet is ready to copy into the Schedule I Nexus Mods upload form after the permission checkpoint in [`docs/NEXUS_UPLOAD_DRAFTS.md`](../NEXUS_UPLOAD_DRAFTS.md) is resolved.

## Main details

**Game**

Schedule I

**Mod name**

Botanists Use X - Soil Pourers and Sprinklers

**One-line summary**

One configurable DLL for botanist soil-pourer and sprinkler automation, including multi-bag pourers and synchronized multiplayer support.

**Version**

2.0.0

**Category**

Employees

**Suggested tags**

Gameplay, Bug Fixes, Quality of Life, Utilities for Players, AI-Generated Content

**Author display**

GSVS UK ACM | Original concepts by Fortis

## General-page field map

- **Mod Name:** `Botanists Use X - Soil Pourers and Sprinklers`
- **Game:** `Schedule I`
- **Category:** `Employees`
- **Mod version:** `2.0.0`
- **Author or team name:** `GSVS UK ACM | Original concepts by Fortis`
- **Short description:** use the one-line summary above
- **Translation of another mod:** leave unchecked
- **Community tagging:** leave enabled
- **Download mirrors:** leave empty
- **Save/Publish:** save and preview only; do not publish until permission and owner approval are recorded

## Requirements

- Schedule I 0.4.7f12 IL2CPP/main branch
- MelonLoader 0.7.0 Open-Beta
- The same `BotanistsUseX.dll` version on every multiplayer participant

No Mod Manager dependency is claimed.

## Full description

Use [`combined-description.md`](combined-description.md). It contains the compatibility callout, feature overview, requirements, installation, configuration, both workflows, multiplayer behavior, testing evidence, troubleshooting, attribution, disclosure, and changelog.

### Rich-text formatting plan

- Format `BOTANISTS USE X` as the largest heading and colour it blue to match the GSVS banner.
- Put the green compatibility callout immediately below the title.
- Format the identity/credit statement as a quote; bold Fortis and GSVS UK ACM.
- Use heading level 2 for major sections and heading level 3 for troubleshooting subsections.
- Preserve numbered requirements/installation and bulleted feature/test lists.
- Make the combined-versus-individual warning a block quote with a bold lead.
- Insert `botanist-equipment-room.png` after the opening section as the hero image.
- Insert the two soil-pourer comparison images beside the Soil Pourer workflow.
- Insert `botanist-device-workflow.png` beside the combined workflow section.
- Insert `mods-folder-location.png` under Installation.
- Use restrained blue and green accents and horizontal rules between major sections.

## Main file

**File title**

Botanists Use X 2.0.0 - Schedule I 0.4.7f12 IL2CPP

**Upload filename**

`BotanistsUseX-2.0.0.zip`

**SHA-256**

`BB249451D15A1BF314A89671DEA8E3536406E444FE85B10946C30B026B73240F`

**File description**

```text
Main combined file. Includes BotanistsUseX.dll, installation README, MIT licence and attribution notice. Requires MelonLoader 0.7.0 Open-Beta. Install this DLL by itself; do not combine it with either individual Botanist Use port. Multiplayer participants should use the same version.
```

**Changelog**

```text
2.0.0
- Ported soil-pourer and sprinkler automation to Schedule I 0.4.7f12 IL2CPP.
- Added one combined DLL with independent feature toggles.
- Added configurable 1-8 bag pourers, persistent charges and one-charge dispensing.
- Added player/botanist top-ups, proportional visuals and charge prompts.
- Added immediate soil-to-seed planting and refill navigation.
- Added small and large sprinkler support with immediate worker release.
- Added host-authoritative multiplayer synchronization and joining-client state.
```

## Permissions and disclosure answers

- **Original work/assets:** Concepts derive from Fortis's BotanistUseSoilPourers and BotanistUseSprinkler. Link both original Nexus pages and source repositories.
- **Permission basis:** Do not claim an open-source licence for the originals. Resolve the repository's permission checkpoint before publication.
- **Other-site upload permission:** Original Nexus permissions currently restrict modified uploads/reuploads; publication remains blocked until documented permission or Nexus confirmation exists.
- **AI disclosure:** Select **AI-Generated Content**. State that Codex assisted implementation and documentation while all work was human-directed, reviewed, built, and play-tested. No generated media is included.
- **Donation Points:** opt out unless/until original-author permission and any arrangement are explicit.
- **Adult content:** no.
- **Paid content:** no.

## Images and captions

Upload in this order:

1. `docs/images/botanist-equipment-room.png`
   **Caption:** Botanists working among linked pots, multi-bag soil pourers and grow equipment.
2. `docs/images/botanist-device-workflow.png`
   **Caption:** A botanist completing a grow task beside linked equipment.
3. `docs/images/soil-pourer-half-full.png`
   **Caption:** Multi-bag pourer showing one of two stored charges.
4. `docs/images/soil-pourer-empty.png`
   **Caption:** Empty pourer ready for its next soil bag.
5. `docs/images/mods-folder-location.png`
   **Caption:** Install BotanistsUseX.dll in the Schedule I Mods folder.

Use `botanist-equipment-room.png` as the current primary image. Replace it with owner-approved combined header art if supplied.

## Recommended links

- Maintained source and issues: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED
- Original Soil Pourer source: https://github.com/fortiiis/BotanistUseSoilPourers
- Original Soil Pourer Nexus page: https://www.nexusmods.com/schedule1/mods/1091
- Original Sprinkler source: https://github.com/fortiiis/BotanistUseSprinkler
- Original Sprinkler Nexus page: https://www.nexusmods.com/schedule1/mods/1080
- MelonLoader: https://melonwiki.xyz/#/

## Pre-publish checklist

- [ ] Resolve and record the Fortis/Nexus permission checkpoint.
- [ ] Keep `Original concepts by Fortis` visible near the top.
- [ ] Set category `Employees`, version `2.0.0`, and the suggested tags.
- [ ] Upload `BotanistsUseX-2.0.0.zip`, not a source archive.
- [ ] Confirm DLL, README, licence, and notice appear in the preview.
- [ ] Apply the rich-text and image plan.
- [ ] Confirm the SHA-256 matches `docs/HANDOVER.md`.
- [ ] Save and preview; publish only after explicit owner approval.
