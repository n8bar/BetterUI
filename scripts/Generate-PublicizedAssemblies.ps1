param(
    [string]$GameDir = "C:\Program Files (x86)\Steam\steamapps\common\Valheim",
    [string]$OutputDir = (Join-Path $PSScriptRoot "..\publicized"),
    [string]$PublicizerVersion = "0.4.3",
    [string]$DotnetExe = $(if (Test-Path "C:\Tools\dotnet8\dotnet.exe") { "C:\Tools\dotnet8\dotnet.exe" } else { "dotnet" })
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$managedDir = Join-Path $GameDir "valheim_Data\Managed"

if (-not (Test-Path $managedDir)) {
    throw "Managed assemblies not found under '$managedDir'. Update GameDir or GameDir.targets first."
}

$tempRoot = Join-Path $env:TEMP "betterui-publicizer"
$pkgRoot = Join-Path $tempRoot $PublicizerVersion
$nupkgPath = Join-Path $tempRoot "BepInEx.AssemblyPublicizer.Cli.$PublicizerVersion.nupkg"
$zipPath = Join-Path $tempRoot "BepInEx.AssemblyPublicizer.Cli.$PublicizerVersion.zip"
$cliDll = Join-Path $pkgRoot "tools\net6.0\any\BepInEx.AssemblyPublicizer.Cli.dll"

New-Item -ItemType Directory -Force -Path $tempRoot | Out-Null
New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

if (-not (Test-Path $cliDll)) {
    $packageUrl = "https://www.nuget.org/api/v2/package/BepInEx.AssemblyPublicizer.Cli/$PublicizerVersion"
    Invoke-WebRequest $packageUrl -OutFile $nupkgPath
    Copy-Item $nupkgPath $zipPath -Force
    if (Test-Path $pkgRoot) {
        Remove-Item -Recurse -Force $pkgRoot
    }
    Expand-Archive -Path $zipPath -DestinationPath $pkgRoot -Force
}

$inputs = @(
    (Join-Path $managedDir "assembly_valheim.dll"),
    (Join-Path $managedDir "assembly_guiutils.dll"),
    (Join-Path $managedDir "assembly_utils.dll")
)

foreach ($input in $inputs) {
    if (-not (Test-Path $input)) {
        throw "Missing required assembly '$input'."
    }
}

& $DotnetExe $cliDll @inputs -o $OutputDir -f

if ($LASTEXITCODE -ne 0) {
    throw "Assembly publicizer failed with exit code $LASTEXITCODE."
}

Write-Host "Publicized assemblies written to $OutputDir"
