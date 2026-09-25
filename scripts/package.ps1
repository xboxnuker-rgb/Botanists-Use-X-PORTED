[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $MelonLoaderRoot,

    [string] $DotNet = "dotnet",

    [string] $Version = "2.0.0"
)

$ErrorActionPreference = "Stop"
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$artifactRoot = Join-Path $repositoryRoot "artifacts"
$buildRoot = Join-Path $repositoryRoot "bin\Il2Cpp"

$resolvedRepositoryRoot = [System.IO.Path]::GetFullPath($repositoryRoot)
$resolvedArtifactRoot = [System.IO.Path]::GetFullPath($artifactRoot)
if (-not $resolvedArtifactRoot.StartsWith($resolvedRepositoryRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Artifact root must remain inside the repository: $resolvedArtifactRoot"
}

function Assert-ArtifactChild([string] $Path) {
    $resolvedPath = [System.IO.Path]::GetFullPath($Path)
    if (-not $resolvedPath.StartsWith($resolvedArtifactRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Package path must remain inside the artifact root: $resolvedPath"
    }
}

& (Join-Path $PSScriptRoot "build-il2cpp.ps1") -MelonLoaderRoot $MelonLoaderRoot -DotNet $DotNet
if ($LASTEXITCODE) { exit $LASTEXITCODE }

New-Item -ItemType Directory -Force -Path $artifactRoot | Out-Null

$packages = @(
    @{ Name = "BotanistUseSoilPourer"; Dll = "BotanistUseSoilPourer.dll" },
    @{ Name = "BotanistUseSprinkler"; Dll = "BotanistUseSprinkler.dll" },
    @{ Name = "BotanistsUseX"; Dll = "BotanistsUseX.dll" }
)

foreach ($package in $packages) {
    $stagePath = Join-Path $artifactRoot "$($package.Name)-$Version"
    Assert-ArtifactChild $stagePath
    if (Test-Path -LiteralPath $stagePath) {
        Remove-Item -LiteralPath $stagePath -Recurse -Force
    }

    New-Item -ItemType Directory -Path $stagePath | Out-Null
    Copy-Item -LiteralPath (Join-Path $buildRoot $package.Dll) -Destination $stagePath
    Copy-Item -LiteralPath (Join-Path $repositoryRoot "README.md") -Destination $stagePath
    Copy-Item -LiteralPath (Join-Path $repositoryRoot "LICENSE.txt") -Destination $stagePath

    $archivePath = Join-Path $artifactRoot "$($package.Name)-$Version.zip"
    Assert-ArtifactChild $archivePath
    if (Test-Path -LiteralPath $archivePath) {
        Remove-Item -LiteralPath $archivePath -Force
    }

    Compress-Archive -Path (Join-Path $stagePath "*") -DestinationPath $archivePath
    $hash = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash
    Write-Output "Packaged $archivePath SHA256 $hash"
}
