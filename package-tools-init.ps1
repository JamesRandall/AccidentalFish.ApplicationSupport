param($installPath, $toolsPath, $package, $project)

if (Get-Module | ?{ $_.Name -eq 'EntityFramework' })
{
    Remove-Module EntityFramework
}

Import-Module (Join-Path $toolsPath net45\AccidentalFish.ApplicationSupport.Powershell.dll)