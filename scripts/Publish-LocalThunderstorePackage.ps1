param(
    [string]$ProfileName = "Default",
    [string]$Community = "Valheim",
    [string]$AuthorName = "n8bar",
    [string]$PackageName = "BetterUI_LocalFork",
    [string]$Version,
    [switch]$BumpPatch,
    [switch]$SkipBuild,
    [string]$DotnetExe
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$buildDll = Join-Path $repoRoot "bin\Release\net48\BetterUI.dll"
$packageRoot = Join-Path $repoRoot "thunderstore\local-package"
$manifestPath = Join-Path $packageRoot "manifest.json"
$dataRoot = Join-Path $env:APPDATA "Thunderstore Mod Manager\DataFolder\$Community"
$modsYmlPath = Join-Path $dataRoot "profiles\$ProfileName\mods.yml"
$bumpScript = Join-Path $PSScriptRoot "Bump-Version.ps1"

if ($BumpPatch) {
    & powershell -ExecutionPolicy Bypass -File $bumpScript -Patch
}
elseif ($Version) {
    & powershell -ExecutionPolicy Bypass -File $bumpScript -Version $Version
}

$manifest = Get-Content $manifestPath -Raw | ConvertFrom-Json
if (-not $Version) {
    $Version = $manifest.version_number
}
if (-not $PackageName) {
    $PackageName = $manifest.name
}

$packageId = "$AuthorName-$PackageName"
$cacheRoot = Join-Path $dataRoot "cache\$AuthorName-$PackageName\$Version"
$cachePlugins = Join-Path $cacheRoot "plugins"
$profilePlugins = Join-Path $dataRoot "profiles\$ProfileName\BepInEx\plugins\$AuthorName-$PackageName"

if (-not $SkipBuild) {
    if (-not $DotnetExe) {
        $preferredDotnet = "C:\Tools\dotnet8\dotnet.exe"
        if (Test-Path $preferredDotnet) {
            $DotnetExe = $preferredDotnet
        }
        else {
            $DotnetExe = "dotnet"
        }
    }

    & $DotnetExe build (Join-Path $repoRoot "BetterUI.csproj") -c Release
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed."
    }
}

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

if (Test-Path $modsYmlPath) {
    $versionParts = $Version.Split(".")
    if ($versionParts.Count -ne 3) {
        throw "Package version '$Version' is not x.y.z."
    }

    $modsText = Get-Content $modsYmlPath -Raw
    $packagePattern = "(?ms)(- manifestVersion: 1\r?\n  name: $([regex]::Escape($packageId))\r?\n.*?  versionNumber:\r?\n    major: )\d+(\r?\n    minor: )\d+(\r?\n    patch: )\d+"
    $packageMatch = [regex]::Match($modsText, $packagePattern)

    if (-not $packageMatch.Success) {
        Write-Warning "Could not find '$packageId' in mods.yml to update version metadata."
    }
    else {
        $updatedModsText = [regex]::Replace($modsText, $packagePattern, {
            param($match)
            "$($match.Groups[1].Value)$($versionParts[0])$($match.Groups[2].Value)$($versionParts[1])$($match.Groups[3].Value)$($versionParts[2])"
        }, 1)

        if ($updatedModsText -ne $modsText) {
            Set-Content $modsYmlPath $updatedModsText
        }
    }
}

Write-Host "Published local Thunderstore package to:"
Write-Host "  Cache:   $cacheRoot"
Write-Host "  Profile: $profilePlugins"
