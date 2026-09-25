# Agent Instructions

This repository is the canonical project memory for Botanists Use X. Keep it usable without chat history.

## Before changing code

1. Read `README.md`, `CONTRIBUTING.md`, and `docs/HANDOVER.md`.
2. Fetch `origin` and inspect open issues and pull requests for overlapping work.
3. Inspect the working tree and preserve unrelated changes.
4. Confirm the exact Schedule I backend and game version represented by the local reference assemblies.

## Project rules

- Never commit game assemblies, generated IL2CPP assemblies, MelonLoader binaries, logs, saves, credentials, local paths, or local toolchains.
- Keep game references outside the repository and pass their root through `MelonLoaderRoot`.
- Treat IL2CPP and Mono as separate compatibility targets. The current project supports Schedule I 0.4.6f13 IL2CPP only.
- Resolve Harmony targets by exact type, method name, and parameter signature. A successful compile is not runtime verification.
- Keep gameplay changes server-authoritative. Clients must receive results through the game's existing networked APIs.
- Keep the combined and individual builds behaviorally aligned. Shared behavior belongs under `Source/Shared`.
- Do not publish a release, install over a user's live mod, merge, or push without explicit owner approval.

## Verification and handover

- Run `scripts/build-il2cpp.ps1` before handing off a build.
- Record reference identities, commands, results, artifact hashes, runtime status, and known limitations in `docs/HANDOVER.md`.
- Keep build output and distributable archives under ignored `bin/` and `artifacts/` directories.
- Use conventional commits and update `CHANGELOG.md` for user-visible changes.
