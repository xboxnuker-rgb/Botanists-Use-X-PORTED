# BOTANISTS USE X

> **✅ WORKS WITH THE SPECIAL CUSTOMERS UPDATE — SCHEDULE I 0.4.7f12**
> Version 2.0.0 has been built and play-tested in single-player and multiplayer on the current IL2CPP release.

> **One configurable DLL for botanist Soil Pourer and Sprinkler automation.**
> Original equipment-automation concepts by **Fortis / Fortis202** · Current implementation maintained by **GSVS UK ACM / xboxnuker-rgb**

Grow equipment should save your botanists time. Botanists Use X lets assigned workers prepare and use multi-bag soil pourers, bring the selected seed and plant it immediately, activate linked small or large sprinklers, and move on instead of repeating the manual version of each job.

The combined DLL provides independent switches for both features. The work remains inside the game's botanist task lifecycle and is host-authoritative in multiplayer.

---

## At a glance

- **Choose either feature:** Soil Pourer and Sprinkler automation have independent settings.
- **Prepare the next crop:** idle botanists refill eligible pourers after their normal planting work.
- **Store multiple bags:** configure each pourer for 1–8 charges; the default is 2.
- **Use one charge at a time:** visible soil and prompts step from 2/2 to 1/2 to empty.
- **Plant without a second trip:** a botanist using stored soil arrives with the configured seed.
- **Use every sprinkler size:** both small and large linked sprinklers are supported.
- **Leave when the device takes over:** workers walk away once a sprinkler starts.
- **Keep vanilla fallbacks:** normal soil and hand-watering tasks remain available when no eligible linked device exists.
- **Stay synchronized:** host capacity, charges, prompts, refills, dispensing, and joining-client state are shared with modded clients.

---

## Requirements

1. **Schedule I 0.4.7f12 — IL2CPP/main branch**
2. **MelonLoader 0.7.0 Open-Beta**
3. **The same `BotanistsUseX.dll` version on the host and every multiplayer client**

No Mod Manager dependency is required. Settings are stored in `UserData/BotanistsUseX/configuration.json`.

The IL2CPP and alternate Mono branches are not interchangeable. Version 2.0.0 makes no Mono compatibility claim.

---

## Installation

1. **Close Schedule I completely.**
2. Open the game installation directory and then its `Mods` folder.
3. Remove the original Fortis DLLs, older Botanists Use X builds, and both individual port DLLs.
4. Extract `BotanistsUseX.dll` from the downloaded ZIP into `Schedule I/Mods`.
5. Launch the game and confirm the blue GSVS `CREATED BY` banner appears in the MelonLoader console.

> **Install the combined DLL by itself. Do not install `BotanistUseSoilPourer.dll` or `BotanistUseSprinkler.dll` beside it.**

In multiplayer, use the same combined DLL and version on every participant.

---

## Configuration

The file is created on first launch:

```json
{
  "soilPourerEnabled": true,
  "autoRefillSoilPourers": true,
  "soilPourerCapacity": 2,
  "allowManualMultiBagFill": true,
  "sprinklerEnabled": true,
  "debug": false
}
```

- Disable either feature without changing DLLs.
- Capacity accepts 1–8 bags.
- Manual multi-bag fill can be disabled separately from botanist automation.
- Detailed debug logging defaults to off.

---

## Soil Pourer workflow

Botanists discover pourers from each device's linked-pot list. A loaded pourer supplies the soil while the botanist carries the pot's selected seed, allowing the worker to dispense and plant during one visit.

When refill automation is enabled, an idle botanist fetches matching soil and walks to an eligible pourer to prepare the next cycle. Players may also top up a partly filled pourer manually. Each use consumes one charge, updates the visible level and prompt, and leaves the remaining charge available.

Charge state persists under `UserData/BotanistsUseX/soil-pourer-charges.json`.

---

## Sprinkler workflow

When a linked small or large sprinkler is ready, the botanist starts that device instead of applying a hand-watering result. The worker skips the watering-can animation and is released to another task as soon as activation succeeds.

If the sprinkler is empty, unavailable, or already unable to water, the game's normal hand-watering path remains available.

---

## Multiplayer

The host owns capacity and charge state. Remote fill and handle requests are validated by the host, and the resulting charge count, prompt, fill level, staged dispense animation, and refill changes are synchronized through existing FishNet RPC paths. Current state is sent to clients joining an existing session.

The host's capacity temporarily overrides a client's local value for that session. The client's own setting remains available when they host or play alone.

---

## Compatibility and testing

Version **2.0.0** was built specifically against **Schedule I 0.4.7f12 IL2CPP** and play-tested through:

- empty, partly filled, and full soil-pourer states;
- player manual filling and staged dispensing;
- botanist stored-soil, seed, and automatic-refill jobs;
- first planting and later refill navigation;
- small and large sprinkler activation;
- worker release after sprinkler activation;
- both combined feature toggles;
- multiplayer host/client charge displays, refills, dispensing, and visual state.

The exact published candidate passed 70 API/Harmony checks, 45 compiled-artifact checks, and a zero-warning release build.

Future Schedule I updates may change these APIs. If this page's game version does not match the installed version, check the maintained release page before reporting a fault.

---

## Troubleshooting

### The mod does not appear

- Confirm `BotanistsUseX.dll` is directly inside `Schedule I/Mods`.
- Confirm MelonLoader 0.7.0 Open-Beta is loading.
- Remove the original and individual Botanist Use DLLs.

### Multiplayer shows one bag or mismatched state

- Install the same version on host and clients.
- Remove every older DLL copy.
- Let the host setting control capacity for the session.

### The botanist uses the normal job

- Confirm the device is linked to that pot.
- Confirm the pourer has compatible soil or the sprinkler has water and is ready.
- Confirm the matching combined feature toggle is enabled.

### The log reports an undefined Harmony target

The DLL targets a different game version. Install the release that names the current Schedule I build.

Include the game version, mod version, host/client role, installed DLL names, reproduction steps, and `MelonLoader/Latest.log` with any reproducible report.

---

## Credits, source and licence

The original **BotanistUseSoilPourers** and **BotanistUseSprinkler** projects were created by **Fortis / Fortis202**.

- Original Soil Pourer Nexus page: https://www.nexusmods.com/schedule1/mods/1091
- Original Soil Pourer source: https://github.com/fortiiis/BotanistUseSoilPourers
- Original Sprinkler Nexus page: https://www.nexusmods.com/schedule1/mods/1080
- Original Sprinkler source: https://github.com/fortiiis/BotanistUseSprinkler

The current combined implementation is maintained by **GSVS UK ACM / xboxnuker-rgb**:

- Source, releases, and issue tracker: https://github.com/xboxnuker-rgb/Botanists-Use-X-PORTED

This is an independently maintained current-game implementation and is not presented as an official Fortis release. The repository's MIT licence covers its new code and documentation only; it does not grant or alter rights in the original projects.

### Development disclosure

Codex assisted compatibility investigation, implementation, and documentation. All changes were human-directed, reviewed, compiled, and repeatedly play-tested. The listing uses Nexus Mods' **AI-Generated Content** tag for disclosure; the supplied screenshots are unedited in-game captures and the mod contains no generated visual, audio, or dialogue assets.

---

## Version 2.0.0 — Special Customers update

- Ported both equipment workflows to Schedule I 0.4.7f12 IL2CPP.
- Added one combined DLL with independent toggles.
- Added persistent, configurable multi-bag pourers and single-charge dispensing.
- Added botanist and manual pourer top-ups with charge-aware prompts and visuals.
- Added immediate soil-to-seed planting and corrected refill navigation.
- Added small and large sprinkler support with immediate worker release.
- Added host-authoritative multiplayer synchronization and joining-client state.
- Added GSVS startup identity and prominent original-author credit.

If the mod improves your grow operation, a clear bug report, useful screenshot, or endorsement helps keep compatibility information accurate.
