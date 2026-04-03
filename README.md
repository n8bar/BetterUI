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

## Versioning

Sync source and package versions:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Bump-Version.ps1 -Version 2.5.10
```

Patch bump:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Bump-Version.ps1 -Patch
```

Publish the local Thunderstore package from the current manifest version:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Publish-LocalThunderstorePackage.ps1
```

That publish step now:

- builds `BetterUI.csproj` in `Release`
- copies the rebuilt DLL into the Thunderstore cache and live profile plugin folder
- updates the local package version metadata in Thunderstore `mods.yml`

Or bump and publish in one step:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Publish-LocalThunderstorePackage.ps1 -BumpPatch
```

## Notes

- `publicized\*.dll` is generated locally and not committed.
- This repo is not a clean upstream source drop. It is a repaired decompile of the maintained Thunderstore build.
- The old public `2.0.2` repo was used as a cleanup guide where the decompiler output was ambiguous.
