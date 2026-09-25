# Contributing

## Source of truth

Read `README.md`, `AGENTS.md`, and `docs/HANDOVER.md` before changing code. Repository documentation and the matching current game assemblies are authoritative. Old mod DLLs, old source, and logs are supporting evidence only.

## Branches and commits

- Branch from refreshed `main` using `<type>/<short-description>`.
- Use conventional commits, for example `feat: port botanist equipment automation`.
- Keep compatibility and gameplay changes narrowly scoped.
- Do not publish or merge without the repository owner's approval.

## Local references

Do not copy proprietary or generated game files into this repository. Pass the matching game installation's `MelonLoader` directory to the build scripts:

```powershell
.\scripts\build-il2cpp.ps1 -MelonLoaderRoot 'C:\Games\Schedule I\MelonLoader'
```

## Pull request checklist

Every pull request should state:

- Exact Schedule I, Unity, MelonLoader, and backend versions tested.
- The `Assembly-CSharp.dll` SHA-256 used for verification.
- Changed compatibility signatures.
- Build/static verification commands and results.
- Runtime scenarios tested, including host/client status.
- Known warnings and limitations.
- Confirmation that no game binaries, logs, saves, credentials, or local paths were committed.
