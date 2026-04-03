param(
    [string]$Version,
    [switch]$Patch
)

$ErrorActionPreference = "Stop"

if (-not $Version -and -not $Patch) {
    throw "Pass -Version x.y.z or -Patch."
}

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$manifestPath = Join-Path $repoRoot "thunderstore\local-package\manifest.json"
$mainPath = Join-Path $repoRoot "BetterUI\Main.cs"
$assemblyInfoPath = Join-Path $repoRoot "Properties\AssemblyInfo.cs"

$manifestText = Get-Content $manifestPath -Raw
$manifestVersionMatch = [regex]::Match($manifestText, '"version_number"\s*:\s*"([^"]+)"')

if (-not $manifestVersionMatch.Success) {
    throw "Could not find version_number in manifest.json."
}

$currentVersion = $manifestVersionMatch.Groups[1].Value

if ($Patch) {
    $parts = $currentVersion.Split('.')
    if ($parts.Count -ne 3) {
        throw "Manifest version '$currentVersion' is not x.y.z."
    }
    $parts[2] = ([int]$parts[2] + 1).ToString()
    $Version = ($parts -join '.')
}

if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    throw "Version must be x.y.z."
}

$manifestText = [regex]::Replace($manifestText, '("version_number"\s*:\s*")[^"]+(")', {
    param($match)
    "$($match.Groups[1].Value)$Version$($match.Groups[2].Value)"
}, 1)
Set-Content $manifestPath $manifestText

$mainText = Get-Content $mainPath -Raw
$mainText = [regex]::Replace(
    $mainText,
    '\[BepInPlugin\("([^"]+)",\s*"([^"]+)",\s*"[^"]+"\)\]',
    "[BepInPlugin(""`$1"", ""`$2"", ""$Version"")]"
)
Set-Content $mainPath $mainText

$assemblyText = Get-Content $assemblyInfoPath -Raw
$assemblyText = [regex]::Replace($assemblyText, 'AssemblyFileVersion\("[^"]+"\)', "AssemblyFileVersion(""$Version"")")
$assemblyText = [regex]::Replace($assemblyText, 'AssemblyVersion\("[^"]+"\)', "AssemblyVersion(""$Version.0"")")
Set-Content $assemblyInfoPath $assemblyText

Write-Host "Synchronized BetterUI version to $Version"
