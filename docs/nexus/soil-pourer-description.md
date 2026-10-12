# BOTANIST USE SOIL POURER PORTED

> **✅ WORKS WITH THE SPECIAL CUSTOMERS UPDATE — SCHEDULE I 0.4.7f12**
> Version 2.0.0 has been built and play-tested in single-player and multiplayer on the current IL2CPP release.

> **The Fortis soil-pourer idea, restored and expanded for the current game.**
> Original mod by **Fortis / Fortis202** · Compatibility implementation maintained by **GSVS UK ACM / xboxnuker-rgb**

Botanist Use Soil Pourer PORTED makes linked pourers part of the botanist's real work cycle. Workers use stored soil, arrive with the selected seed, plant immediately, and prepare pourers during idle time for the next crop.

The port also adds configurable multi-bag storage. A pourer can hold 1–8 charges, shows how full it is, accepts matching top-ups, and dispenses one charge instead of emptying everything at once.

---

## At a glance

- **Use linked pourers:** botanists discover devices from their authoritative linked-pot lists.
- **Carry 1–8 bags:** capacity is configurable and defaults to 2.
- **Show the real amount:** fill height and interaction text follow the stored charge count.
- **Dispense once per pot:** a 2/2 pourer steps to 1/2 and then empty.
- **Bring the seed:** stored-soil jobs carry the configured seed and plant immediately.
- **Prepare ahead:** idle botanists refill eligible pourers for the next planting cycle.
- **Let players top up:** partly filled devices accept another bag of matching soil.
- **Keep state synchronized:** the host owns charge/capacity state and sends it to clients, including clients joining later.

---

## Requirements

1. **Schedule I 0.4.7f12 — IL2CPP/main branch**
2. **MelonLoader 0.7.0 Open-Beta**
3. **The same `BotanistUseSoilPourer.dll` version on every multiplayer participant**

No Mod Manager dependency is required. Configuration is stored under `UserData/BotanistUseSoilPourer`.

Mono/alternate is not supported by version 2.0.0.

---

## Installation

1. **Close Schedule I completely.**
2. Open the game's installation directory and its `Mods` folder.
3. Remove the original Fortis Soil Pourer DLL and any older port.
4. Extract `BotanistUseSoilPourer.dll` into `Schedule I/Mods`.
5. Do not install `BotanistsUseX.dll` beside it.
6. Launch the game and confirm the blue GSVS `PORTED BY` banner appears.

You may install the separate Sprinkler port beside this individual DLL. In multiplayer, use the same layout and version on every participant.

---

## Configuration

`UserData/BotanistUseSoilPourer/configuration.json` contains:

- `enabled`
- `autoRefillSoilPourers`
- `soilPourerCapacity` (1–8, default 2)
- `allowManualMultiBagFill`
- `debug`

Charge state persists in `UserData/BotanistUseSoilPourer/soil-pourer-charges.json`.

---

## How it works

With stored soil available, a botanist fetches the pot's configured seed rather than another soil bag. At the linked pot, the pourer supplies one charge and the worker plants the carried seed during the same action.

When a pourer needs preparation, an idle botanist fetches matching soil and walks to the pourer for a refill-only trip. The first planting trip still targets the pot. Normal soil behavior remains available when no eligible linked pourer exists.

Players can load the first bag normally and add further matching bags while the prompt shows the current count. The visible soil level follows the configured fraction.

---

## Multiplayer

The host owns capacity and per-pourer charges. Remote fill and handle interactions run through host validation. Charge prompts, proportional visuals, refill events, staged dispensing, and the current state for joining clients are synchronized through existing game RPC paths.

The host capacity temporarily replaces a client's local value during the session.

---

## Compatibility and testing

Version **2.0.0** was built against **Schedule I 0.4.7f12 IL2CPP** and play-tested for:

- native first-bag filling;
- manual 1/2 to 2/2 top-up;
- 2/2 to 1/2 to empty dispensing;
- refill interaction after partial use;
- botanist stored-soil and immediate seed planting;
- automatic pourer preparation;
- pot versus pourer navigation;
- normal fallback without a linked device;
- multiplayer host/client state and interactions.

Future game updates may change these APIs. Match the release's named game version before reporting a fault.

---

## Troubleshooting

### The pourer still holds only one bag

- Confirm host and clients all use version 2.0.0.
- Remove original and older DLL copies.
- Confirm `soilPourerCapacity` is greater than 1 in the host configuration.

### A second soil bag is rejected

- Confirm manual multi-bag filling is enabled.
- Use the same soil type already stored in the pourer.
- Confirm the pourer is below its configured capacity.

### The botanist uses a bag directly on the pot

- Confirm that pourer is linked to the target pot.
- Confirm the feature is enabled and the device is eligible.
- Include the MelonLoader log if the link is valid but the fallback still occurs.

### The log reports an undefined Harmony target

Install the release that explicitly names the installed Schedule I version.

---

## Why the name says PORTED

The original release targeted botanist behavior APIs that were later removed. **PORTED** distinguishes this current-game compatibility implementation from the original and helps players avoid loading both DLLs together.

It does not replace or erase Fortis's authorship of the original mod.

---

## Credits, source and licence

**BotanistUseSoilPourers** was created by **Fortis / Fortis202**.

- Original Nexus page: https://www.nexusmods.com/schedule1/mods/1091
- Original source: https://github.com/fortiiis/BotanistUseSoilPourers
- Maintained implementation: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED

This is an independently maintained implementation and is not presented as an official Fortis release. The repository MIT licence covers new repository code and documentation only.

### Development disclosure

Codex assisted compatibility investigation, implementation, and documentation. All changes were human-directed, reviewed, compiled, and play-tested. Select Nexus Mods' **AI-Generated Content** tag; the supplied screenshots are unedited in-game captures.

---

## Version 2.0.0 — Special Customers update

- Ported soil-pourer automation to the current grow-container API.
- Added configurable persistent multi-bag capacity.
- Added manual and botanist top-ups, charge prompts, and proportional fill visuals.
- Added one-charge staged dispensing and partial-state refill interaction.
- Added immediate seed planting after stored-soil use.
- Added idle preparation and correct refill navigation.
- Added host-authoritative multiplayer synchronization.
- Added GSVS `PORTED BY` startup identity with Fortis credit.
