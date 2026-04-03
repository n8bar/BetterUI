# BetterUI 2.5.9 Source Fork

This branch rebuilds `BetterUI_ForeverMaintained` `2.5.9` from decompiled source and keeps the local enemy HUD changes that were previously done as binary patches:

- enemy level display uses `m_level - 1`
- non-zero enemy levels render as repeated `★` instead of `Lv.#`

## Status

- source base: decompiled `BetterUI_ForeverMaintained` `2.5.9`
- branch: `fm-2.5.9`
- current build: `dotnet build BetterUI.csproj -c Release`

## Requirements

- .NET SDK `8.0.x`
- Valheim install
- Thunderstore/BepInEx profile with the standard Valheim `BepInEx` core DLLs

## Build

1. Adjust `GameDir.targets` if your Valheim install or profile paths differ from the defaults.
2. Generate the publicized game assemblies:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Generate-PublicizedAssemblies.ps1
```

3. Build:

```powershell
dotnet build .\BetterUI.csproj -c Release
```

Output DLL:

```text
bin\Release\net48\BetterUI.dll
```

## Notes

- `publicized\*.dll` is generated locally and not committed.
- This repo is not a clean upstream source drop. It is a repaired decompile of the maintained Thunderstore build.
- The old public `2.0.2` repo was used as a cleanup guide where the decompiler output was ambiguous.
