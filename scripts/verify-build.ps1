[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $MelonLoaderRoot,

    [Parameter(Mandatory = $true)]
    [string] $OutputDirectory
)

$ErrorActionPreference = "Stop"
$script:PassCount = 0
$cecilPath = Join-Path $MelonLoaderRoot "net6\Mono.Cecil.dll"
[void] [System.Reflection.Assembly]::LoadFrom($cecilPath)

$expectations = @(
    @{ File = "BotanistUseSoilPourer.dll"; Soil = $true; Sprinkler = $false; ConfigType = "BotanistUseSoilPourer.Configuration"; EntryType = "BotanistUseSoilPourer.BotanistUseSoilPourerMod"; BannerName = "Botanist Use Soil Pourer"; BannerFeatures = "Soil Pourer" },
    @{ File = "BotanistUseSprinkler.dll"; Soil = $false; Sprinkler = $true; ConfigType = "BotanistUseSprinkler.Configuration"; EntryType = "BotanistUseSprinkler.BotanistUseSprinklerMod"; BannerName = "Botanist Use Sprinkler"; BannerFeatures = "Sprinkler" },
    @{ File = "BotanistsUseX.dll"; Soil = $true; Sprinkler = $true; ConfigType = "BotanistsUseX.Configuration"; EntryType = "BotanistsUseX.BotanistsUseXMod"; BannerName = "Botanists Use X"; BannerFeatures = "Soil Pourer + Sprinkler" }
)

function Assert-Condition([bool] $condition, [string] $message) {
    if (-not $condition) {
        throw $message
    }

    Write-Output "PASS $message"
    $script:PassCount++
}

foreach ($expectation in $expectations) {
    $assemblyPath = Join-Path $OutputDirectory $expectation.File
    Assert-Condition (Test-Path -LiteralPath $assemblyPath -PathType Leaf) "built $($expectation.File)"

    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($assemblyPath)
    try {
        $types = @($assembly.MainModule.GetTypes())
        $allReferences = @(
            $assembly.MainModule.GetTypeReferences() | ForEach-Object { $_.FullName }
            $types | ForEach-Object {
                $_.CustomAttributes | ForEach-Object {
                    $_.ConstructorArguments | ForEach-Object { [string] $_.Value }
                }
            }
        )

        Assert-Condition (-not ($allReferences -match "PotActionBehaviour")) "$($expectation.File) has no obsolete PotActionBehaviour references"

        $soilPatch = $types | Where-Object { $_.FullName -ceq "BotanistsUseX.Runtime.SoilPourerActionSuccessPatch" }
        $sprinklerPatch = $types | Where-Object { $_.FullName -ceq "BotanistsUseX.Runtime.SprinklerActionSuccessPatch" }
        Assert-Condition (($null -ne $soilPatch) -eq $expectation.Soil) "$($expectation.File) soil patch composition is correct"
        Assert-Condition (($null -ne $sprinklerPatch) -eq $expectation.Sprinkler) "$($expectation.File) sprinkler patch composition is correct"

        $configuration = $types | Where-Object { $_.FullName -ceq $expectation.ConfigType }
        Assert-Condition ($null -ne $configuration) "$($expectation.File) contains its configuration contract"

        $banner = $types | Where-Object { $_.FullName -ceq "BotanistsUseX.Runtime.StartupBanner" }
        Assert-Condition ($null -ne $banner) "$($expectation.File) contains the shared startup banner"

        $entryType = $types | Where-Object { $_.FullName -ceq $expectation.EntryType }
        $initializeMethod = $entryType.Methods | Where-Object { $_.Name -ceq "OnInitializeMelon" } | Select-Object -First 1
        $initializeInstructions = @($initializeMethod.Body.Instructions)
        $bannerCalls = @($initializeInstructions | Where-Object {
            $_.OpCode.Code -eq [Mono.Cecil.Cil.Code]::Call -and
            $_.Operand.FullName -ceq "System.Void BotanistsUseX.Runtime.StartupBanner::Print(System.String,System.String)"
        })
        Assert-Condition ($bannerCalls.Count -eq 1) "$($expectation.File) prints its startup banner exactly once"

        $startupStrings = @($initializeInstructions | Where-Object {
            $_.OpCode.Code -eq [Mono.Cecil.Cil.Code]::Ldstr
        } | ForEach-Object { [string] $_.Operand })
        Assert-Condition (
            $expectation.BannerName -cin $startupStrings -and $expectation.BannerFeatures -cin $startupStrings
        ) "$($expectation.File) embeds the correct banner identity"

        $harmonyTargets = @($types.CustomAttributes | Where-Object { $_.AttributeType.FullName -ceq "HarmonyLib.HarmonyPatch" })
        Assert-Condition ($harmonyTargets.Count -gt 0) "$($expectation.File) contains Harmony patch metadata"

        $actualTargets = @($harmonyTargets | ForEach-Object {
            $targetType = $_.ConstructorArguments[0].Value.FullName
            $targetMethod = [string] $_.ConstructorArguments[1].Value
            "$targetType::$targetMethod"
        })

        $expectedTargets = @()
        if ($expectation.Soil) {
            $expectedTargets += @(
                "Il2CppScheduleOne.NPCs.Behaviour.AddSoilToGrowContainerBehaviour::GetRequiredItemSuitableIDs",
                "Il2CppScheduleOne.NPCs.Behaviour.AddSoilToGrowContainerBehaviour::GetActionDuration",
                "Il2CppScheduleOne.NPCs.Behaviour.AddSoilToGrowContainerBehaviour::OnActionSuccess"
            )
        }
        if ($expectation.Sprinkler) {
            $expectedTargets += @(
                "Il2CppScheduleOne.NPCs.Behaviour.WaterPotBehaviour::GetActionDuration",
                "Il2CppScheduleOne.NPCs.Behaviour.WaterPotBehaviour::OnActionSuccess"
            )
        }

        $unexpectedTargets = @($actualTargets | Where-Object { $_ -notin $expectedTargets })
        $missingTargets = @($expectedTargets | Where-Object { $_ -notin $actualTargets })
        Assert-Condition ($unexpectedTargets.Count -eq 0) "$($expectation.File) has no unexpected Harmony targets"
        Assert-Condition ($missingTargets.Count -eq 0) "$($expectation.File) contains every expected Harmony target"

        $hash = (Get-FileHash -LiteralPath $assemblyPath -Algorithm SHA256).Hash
        Write-Output "PASS $($expectation.File) SHA256 $hash"
        $script:PassCount++
    }
    finally {
        $assembly.Dispose()
    }
}

Write-Output "Botanists Use X build verification passed ($script:PassCount checks)."
