[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $MelonLoaderRoot,

    [string] $DotNet = "dotnet"
)

$ErrorActionPreference = "Stop"
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $repositoryRoot "BotanistsUseX.sln"
$outputDirectory = Join-Path $repositoryRoot "bin\Il2Cpp"

& (Join-Path $PSScriptRoot "verify-game-api.ps1") -MelonLoaderRoot $MelonLoaderRoot
if ($LASTEXITCODE) { exit $LASTEXITCODE }

& $DotNet build $solutionPath `
    --configuration Release `
    --property:MelonLoaderRoot="$MelonLoaderRoot" `
    --nologo
if ($LASTEXITCODE) { exit $LASTEXITCODE }

& (Join-Path $PSScriptRoot "verify-build.ps1") `
    -MelonLoaderRoot $MelonLoaderRoot `
    -OutputDirectory $outputDirectory
if ($LASTEXITCODE) { exit $LASTEXITCODE }

Write-Output "Built and verified all IL2CPP outputs in $outputDirectory"
