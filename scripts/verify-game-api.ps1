[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $MelonLoaderRoot,

    [string] $ExpectedAssemblySha256 = "0D2EB364F3E84120AF7CCC9FA6BAFD597D42D495EBACC3A260CB4CA0CF0513DA"
)

$ErrorActionPreference = "Stop"
$script:PassCount = 0

$requiredReferences = @(
    "net6\MelonLoader.dll",
    "net6\0Harmony.dll",
    "net6\Mono.Cecil.dll",
    "net6\Il2CppInterop.Runtime.dll",
    "net6\Il2CppInterop.Common.dll",
    "net6\Il2CppInterop.HarmonySupport.dll",
    "Il2CppAssemblies\Assembly-CSharp.dll",
    "Il2CppAssemblies\Il2CppScheduleOne.Core.dll",
    "Il2CppAssemblies\Il2Cppmscorlib.dll",
    "Il2CppAssemblies\Il2CppFishNet.Runtime.dll",
    "Il2CppAssemblies\UnityEngine.dll",
    "Il2CppAssemblies\UnityEngine.CoreModule.dll"
)

foreach ($relativePath in $requiredReferences) {
    $referencePath = Join-Path $MelonLoaderRoot $relativePath
    if (-not (Test-Path -LiteralPath $referencePath -PathType Leaf)) {
        throw "Missing required IL2CPP build reference: $referencePath"
    }
}
Write-Output "PASS reference layout contains all required IL2CPP build assemblies"
$script:PassCount++

$assemblyPath = Join-Path $MelonLoaderRoot "Il2CppAssemblies\Assembly-CSharp.dll"
$actualHash = (Get-FileHash -LiteralPath $assemblyPath -Algorithm SHA256).Hash
if ($ExpectedAssemblySha256 -and $actualHash -cne $ExpectedAssemblySha256) {
    throw "Assembly-CSharp.dll hash mismatch. Expected $ExpectedAssemblySha256, found $actualHash."
}

Write-Output "PASS Assembly-CSharp.dll SHA256 $actualHash"
$script:PassCount++

$cecilPath = Join-Path $MelonLoaderRoot "net6\Mono.Cecil.dll"
[void] [System.Reflection.Assembly]::LoadFrom($cecilPath)
$assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($assemblyPath)

try {
    function Get-Type([string] $fullName) {
        return $assembly.MainModule.GetTypes() |
            Where-Object { $_.FullName -ceq $fullName } |
            Select-Object -First 1
    }

    function Assert-Type([string] $fullName, [string] $baseType = "") {
        $type = Get-Type $fullName
        if (-not $type) {
            throw "Missing game type: $fullName"
        }

        if ($baseType -and $type.BaseType.FullName -cne $baseType) {
            throw "Expected $fullName to derive from $baseType; found $($type.BaseType.FullName)."
        }

        Write-Output "PASS type $fullName"
        $script:PassCount++
    }

    function Assert-MissingType([string] $fullName) {
        if (Get-Type $fullName) {
            throw "Obsolete type unexpectedly exists: $fullName"
        }

        Write-Output "PASS obsolete type remains absent: $fullName"
        $script:PassCount++
    }

    function Assert-Method(
        [string] $typeName,
        [string] $methodName,
        [string] $returnType,
        [string[]] $parameterTypes = @()
    ) {
        $type = Get-Type $typeName
        if (-not $type) {
            throw "Missing type while checking method: $typeName"
        }

        $matches = @($type.Methods | Where-Object {
            if ($_.Name -cne $methodName -or $_.Parameters.Count -ne $parameterTypes.Count) {
                return $false
            }

            for ($index = 0; $index -lt $parameterTypes.Count; $index++) {
                if ($_.Parameters[$index].ParameterType.FullName -cne $parameterTypes[$index]) {
                    return $false
                }
            }

            return $true
        })

        if ($matches.Count -ne 1) {
            throw "Expected one method $typeName::$methodName($($parameterTypes -join ', ')); found $($matches.Count)."
        }

        if ($matches[0].ReturnType.FullName -cne $returnType) {
            throw "Expected $typeName::$methodName to return $returnType; found $($matches[0].ReturnType.FullName)."
        }

        Write-Output "PASS method $typeName::$methodName($($parameterTypes -join ', ')) -> $returnType"
        $script:PassCount++
    }

    function Assert-Property(
        [string] $typeName,
        [string] $propertyName,
        [string] $propertyType
    ) {
        $type = Get-Type $typeName
        $property = $type.Properties |
            Where-Object { $_.Name -ceq $propertyName -and $_.PropertyType.FullName -ceq $propertyType } |
            Select-Object -First 1

        if (-not $property) {
            throw "Missing property $typeName::$propertyName : $propertyType"
        }

        Write-Output "PASS property $typeName::$propertyName : $propertyType"
        $script:PassCount++
    }

    $growBehavior = "Il2CppScheduleOne.NPCs.Behaviour.GrowContainerBehaviour"
    $soilBehavior = "Il2CppScheduleOne.NPCs.Behaviour.AddSoilToGrowContainerBehaviour"
    $waterBehavior = "Il2CppScheduleOne.NPCs.Behaviour.WaterPotBehaviour"
    $growContainer = "Il2CppScheduleOne.Growing.GrowContainer"
    $itemInstance = "Il2CppScheduleOne.ItemFramework.ItemInstance"
    $stringArray = "Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStringArray"

    Assert-MissingType "Il2CppScheduleOne.NPCs.Behaviour.PotActionBehaviour"
    Assert-Type $growBehavior "Il2CppScheduleOne.NPCs.Behaviour.Behaviour"
    Assert-Type $soilBehavior $growBehavior
    Assert-Type $waterBehavior $growBehavior
    Assert-Type "Il2CppScheduleOne.ObjectScripts.SoilPourer" "Il2CppScheduleOne.EntityFramework.GridItem"
    Assert-Type "Il2CppScheduleOne.ObjectScripts.Sprinkler" "Il2CppScheduleOne.EntityFramework.GridItem"
    Assert-Type "Il2CppScheduleOne.ObjectScripts.Pot" $growContainer

    Assert-Property $growBehavior "_growContainer" $growContainer
    Assert-Method $soilBehavior "GetRequiredItemSuitableIDs" $stringArray @($growContainer)
    Assert-Method $soilBehavior "GetActionDuration" "System.Single"
    Assert-Method $soilBehavior "OnActionSuccess" "System.Void" @($itemInstance)
    Assert-Method $waterBehavior "GetActionDuration" "System.Single"
    Assert-Method $waterBehavior "OnActionSuccess" "System.Void" @($itemInstance)

    Assert-Property "Il2CppScheduleOne.ObjectScripts.SoilPourer" "SoilID" "System.String"
    Assert-Method "Il2CppScheduleOne.ObjectScripts.SoilPourer" "GetPots" 'Il2CppSystem.Collections.Generic.List`1<Il2CppScheduleOne.ObjectScripts.Pot>'
    Assert-Method "Il2CppScheduleOne.ObjectScripts.SoilPourer" "SendSoil" "System.Void" @("System.String")
    Assert-Method "Il2CppScheduleOne.ObjectScripts.SoilPourer" "SetSoilLevel" "System.Void" @("System.Single")

    Assert-Property "Il2CppScheduleOne.ObjectScripts.Sprinkler" "IsSprinkling" "System.Boolean"
    Assert-Method "Il2CppScheduleOne.ObjectScripts.Sprinkler" "CanWater" "System.Boolean"
    Assert-Method "Il2CppScheduleOne.ObjectScripts.Sprinkler" "SendWater" "System.Void"
    Assert-Method "Il2CppScheduleOne.ObjectScripts.Sprinkler" "GetPots" 'Il2CppSystem.Collections.Generic.List`1<Il2CppScheduleOne.ObjectScripts.Pot>'

    Assert-Method $growContainer "SetSoil" "System.Void" @("Il2CppScheduleOne.ItemFramework.SoilDefinition")
    Assert-Method $growContainer "SetSoilAmount" "System.Void" @("System.Single")
    Assert-Method $growContainer "SetRemainingSoilUses" "System.Void" @("System.Int32")
    Assert-Method $growContainer "SyncSoilData" "System.Void"

    Write-Output "Schedule I 0.4.6f13 IL2CPP Botanists Use X API verification passed ($script:PassCount checks)."
}
finally {
    $assembly.Dispose()
}
