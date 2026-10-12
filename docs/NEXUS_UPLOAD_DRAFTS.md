# Nexus Mods publication drafts

This directory follows the same release-page pattern used by Improved Packagers PORTED: a polished rich-text description plus a field-by-field upload sheet for every public listing.

Nothing here has been published.

## Listing map

| Planned listing | Full description | Upload form and checklist |
| --- | --- | --- |
| Botanists Use X — combined mod | [Combined description](nexus/combined-description.md) | [Combined upload sheet](nexus/combined-upload.md) |
| Botanist Use Soil Pourer PORTED | [Soil Pourer description](nexus/soil-pourer-description.md) | [Soil Pourer upload sheet](nexus/soil-pourer-upload.md) |
| Botanist Use Sprinkler PORTED | [Sprinkler description](nexus/sprinkler-description.md) | [Sprinkler upload sheet](nexus/sprinkler-upload.md) |

## Shared presentation style

- Open with a blue largest heading matching the GSVS startup banner.
- Put a green **WORKS WITH THE SPECIAL CUSTOMERS UPDATE — SCHEDULE I 0.4.7f12** callout immediately below it.
- Follow with one short identity/credit callout before the longer introduction.
- Use `At a glance`, `Requirements`, `Installation`, feature/usage, `Compatibility and testing`, `Troubleshooting`, credits/disclosure, and changelog sections in that order.
- Use restrained blue and green accents while leaving body text in the Nexus default colour.
- Separate major sections with horizontal rules.
- Put the exact original-author credit near the top and again in the credits section.
- Use the unedited supplied screenshots; do not mark them as generated media.
- Select Nexus's **AI-Generated Content** disclosure tag because Codex assisted code and documentation.

## Shared requirements

- Schedule I `0.4.7f12` on the IL2CPP/main branch.
- MelonLoader `0.7.0 Open-Beta`.
- The same chosen DLL layout and version for every multiplayer participant.
- No Mod Manager dependency is claimed; configuration is stored in JSON under `UserData`.

## Permission checkpoint — required before publication

The original [BotanistUseSoilPourers Nexus page](https://www.nexusmods.com/schedule1/mods/1091) and [BotanistUseSprinkler Nexus page](https://www.nexusmods.com/schedule1/mods/1080) currently reserve modification and redistribution permission. Their public GitHub repositories do not declare a software licence.

Before publishing either port or the combined listing, the owner must do one of the following and retain the evidence:

1. Obtain explicit permission from Fortis / Fortis202 for the planned Nexus uploads; or
2. Obtain written confirmation from Nexus Mods that this independently maintained, separately branded implementation may be listed despite the original-page restrictions.

Do not describe the original projects as MIT licensed. Do not claim endorsement. Do not publish while this checkpoint remains unresolved.

This repository's `LICENSE.txt` applies to new repository code and documentation only; [NOTICE.txt](../NOTICE.txt) records the third-party attribution boundary.

## Shared image inventory

Upload only the images relevant to each listing:

1. `docs/images/botanist-equipment-room.png`
   **Caption:** Botanists working among linked pots and grow equipment.
2. `docs/images/botanist-device-workflow.png`
   **Caption:** A botanist completing a grow task beside linked equipment.
3. `docs/images/soil-pourer-half-full.png`
   **Caption:** Multi-bag soil pourer showing one of two stored charges.
4. `docs/images/soil-pourer-empty.png`
   **Caption:** Empty soil pourer ready to accept a new soil bag.
5. `docs/images/mods-folder-location.png`
   **Caption:** Open the Schedule I installation directory and place the chosen DLL in `Mods`.

Recommended primary images:

- Combined: `botanist-equipment-room.png`.
- Soil Pourer: `soil-pourer-half-full.png`.
- Sprinkler: wait for a dedicated small/large sprinkler activation image before publication.

Branded header art for all three listings and a dedicated sprinkler action screenshot still await owner prompts.

## Shared disclosure and form choices

- Game: Schedule I.
- Category: Employees.
- Suggested tags: Gameplay, Bug Fixes, Quality of Life, Utilities for Players, AI-Generated Content.
- Translation: no.
- Adult content: no.
- Paid content: no.
- Community tagging: enabled.
- Download mirrors: empty; Nexus hosts the verified main file and GitHub remains the source/issue reference.
- Donation Points: opt out unless/until original-author permission and any desired arrangement are explicit.
- Do not select any event whose rules exclude AI-assisted code.

## Shared final checklist

- [ ] Resolve and document the permission checkpoint.
- [ ] Add the owner-approved branded header and listing thumbnail.
- [ ] Add a dedicated sprinkler screenshot to the sprinkler listing.
- [ ] Upload the named ZIP from `artifacts`, never GitHub's source-code archive.
- [ ] Confirm the ZIP preview includes the DLL, `README.md`, `LICENSE.txt`, and `NOTICE.txt`.
- [ ] Apply the rich-text formatting and image order from the listing-specific upload sheet.
- [ ] Verify the visible original-author and maintainer credit.
- [ ] Recheck the uploaded file's SHA-256 against `docs/HANDOVER.md`.
- [ ] Save and preview each page; leave final **Publish** approval to the owner.
