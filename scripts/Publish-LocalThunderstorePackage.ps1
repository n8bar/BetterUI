param(
    [string]$ProfileName = "Default",
    [string]$Community = "Valheim",
    [string]$AuthorName = "n8bar",
    [string]$PackageName = "BetterUI_LocalFork",
    [string]$Version = "2.5.10"
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$buildDll = Join-Path $repoRoot "bin\Release\net48\BetterUI.dll"
$packageRoot = Join-Path $repoRoot "thunderstore\local-package"
$dataRoot = Join-Path $env:APPDATA "Thunderstore Mod Manager\DataFolder\$Community"
$cacheRoot = Join-Path $dataRoot "cache\$AuthorName-$PackageName\$Version"
$cachePlugins = Join-Path $cacheRoot "plugins"
$profilePlugins = Join-Path $dataRoot "profiles\$ProfileName\BepInEx\plugins\$AuthorName-$PackageName"

if (-not (Test-Path $buildDll)) {
    throw "Build output not found at '$buildDll'. Run 'dotnet build .\\BetterUI.csproj -c Release' first."
}

if (-not (Test-Path $packageRoot)) {
    throw "Package template folder not found at '$packageRoot'."
}

New-Item -ItemType Directory -Force -Path $cachePlugins | Out-Null
New-Item -ItemType Directory -Force -Path $profilePlugins | Out-Null

Copy-Item (Join-Path $packageRoot "manifest.json") $cacheRoot -Force
Copy-Item (Join-Path $packageRoot "README.md") $cacheRoot -Force
Copy-Item (Join-Path $repoRoot "LICENSE") $cacheRoot -Force

$iconSource = "C:\Users\n8Bar\AppData\Roaming\Thunderstore Mod Manager\DataFolder\Valheim\cache\BetterUI_ForeverMaintained-BetterUI_ForeverMaintained\2.5.9\icon.png"
if (Test-Path $iconSource) {
    Copy-Item $iconSource (Join-Path $cacheRoot "icon.png") -Force
}

Copy-Item $buildDll (Join-Path $cachePlugins "BetterUI.dll") -Force
Copy-Item $buildDll (Join-Path $profilePlugins "BetterUI.dll") -Force

Write-Host "Published local Thunderstore package to:"
Write-Host "  Cache:   $cacheRoot"
Write-Host "  Profile: $profilePlugins"
