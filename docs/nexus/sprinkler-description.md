# BOTANIST USE SPRINKLER PORTED

> **✅ WORKS WITH THE SPECIAL CUSTOMERS UPDATE — SCHEDULE I 0.4.7f12**
> Version 2.0.0 has been built and play-tested on the current IL2CPP release.

> **The Fortis sprinkler idea, restored for small and large current-game devices.**
> Original mod by **Fortis / Fortis202** · Compatibility implementation maintained by **GSVS UK ACM / xboxnuker-rgb**

Botanist Use Sprinkler PORTED makes assigned workers activate ready linked sprinklers instead of standing at each pot with a watering can. Both small and large sprinklers are supported, and the botanist walks away as soon as the device takes over.

---

## At a glance

- **Use both sizes:** small and large sprinklers are supported.
- **Use real device links:** discovery follows each sprinkler's authoritative linked-pot list.
- **Skip hand watering:** the sprinkler provides the watering result when ready.
- **Release the worker:** the botanist leaves immediately after activation.
- **Avoid duplicate work:** a pot is not immediately reassigned while its sprinkler is already running.
- **Keep the fallback:** ordinary hand watering remains available when no usable linked sprinkler exists.
- **Keep server authority:** activation occurs through the normal networked device API.

---

## Requirements

1. **Schedule I 0.4.7f12 — IL2CPP/main branch**
2. **MelonLoader 0.7.0 Open-Beta**
3. **The same `BotanistUseSprinkler.dll` version on multiplayer participants**

No Mod Manager dependency is required. Configuration is stored under `UserData/BotanistUseSprinkler`.

Mono/alternate is not supported by version 2.0.0.

---

## Installation

1. **Close Schedule I completely.**
2. Open the game installation directory and its `Mods` folder.
3. Remove the original Fortis Sprinkler DLL and any older port.
4. Extract `BotanistUseSprinkler.dll` into `Schedule I/Mods`.
5. Do not install `BotanistsUseX.dll` beside it.
6. Launch the game and confirm the blue GSVS `PORTED BY` banner appears.

You may install the separate Soil Pourer port beside this individual DLL. For consistent multiplayer support, use the same layout and version on every participant.

---

## Configuration

`UserData/BotanistUseSprinkler/configuration.json` contains:

- `enabled`
- `debug`

Valid configuration files are rewritten at startup so new settings can be added without deleting the existing file.

---

## How it works

When the botanist reaches a pot that needs water, the port checks the linked sprinklers. If a device is ready and able to water, the host activates it through the game's normal network API.

The hand-held watering equipment and animation are skipped because the sprinkler owns the action. The botanist is then released instead of standing beside the pot while the device waters it.

If no linked sprinkler can water, the game continues with normal hand watering.

---

## Compatibility and testing

Version **2.0.0** was built against **Schedule I 0.4.7f12 IL2CPP** and play-tested for:

- small sprinkler discovery and activation;
- large sprinkler discovery and activation;
- immediate worker release;
- already-active sprinkler task suppression;
- normal hand-watering fallback;
- the individual and combined build variants;
- multiplayer use with the final shared implementation.

The release candidate passed exact game API/Harmony verification and compiled-artifact inspection with zero build warnings or errors.

Future game updates may change these APIs. Match the release's named game version before reporting a fault.

---

## Troubleshooting

### The botanist still uses a watering can

- Confirm the sprinkler is linked to that pot.
- Confirm it contains water and is ready to activate.
- Confirm this feature is enabled and no duplicate/legacy DLL is installed.

### The botanist stays beside the pot

- Remove every older Sprinkler DLL.
- Confirm the console identifies version 2.0.0.
- Include the MelonLoader log and whether the device was small or large in the report.

### The log reports an undefined Harmony target

Install the release that explicitly names the installed Schedule I version.

---

## Why the name says PORTED

The original release targeted botanist behavior APIs that were later removed. **PORTED** distinguishes this current-game compatibility implementation from the original and helps players avoid loading both DLLs together.

It does not replace or erase Fortis's authorship of the original mod.

---

## Credits, source and licence

**BotanistUseSprinkler** was created by **Fortis / Fortis202**.

- Original Nexus page: https://www.nexusmods.com/schedule1/mods/1080
- Original source: https://github.com/fortiiis/BotanistUseSprinkler
- Maintained implementation: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED

This is an independently maintained implementation and is not presented as an official Fortis release. The repository MIT licence covers new repository code and documentation only.

### Development disclosure

Codex assisted compatibility investigation, implementation, and documentation. All changes were human-directed, reviewed, compiled, and play-tested. Select Nexus Mods' **AI-Generated Content** tag; the supplied screenshots are unedited in-game captures.

---

## Version 2.0.0 — Special Customers update

- Ported sprinkler automation to the current grow-container API.
- Added authoritative linked-device discovery for small and large sprinklers.
- Removed the redundant watering-can animation when a sprinkler owns the action.
- Released the botanist immediately after successful activation.
- Prevented duplicate watering jobs while a sprinkler is active.
- Preserved normal hand-watering fallback.
- Added GSVS `PORTED BY` startup identity with Fortis credit.
