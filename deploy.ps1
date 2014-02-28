#.\build-tools.ps1

$webconfig = $location.tostring() + "\AccidentalFish.Operations.Website\web.config"
$csdef = $location.tostring() + "\AccidentalFish.Operations.Cloud\ServiceDefinition.csdef"
$cloudcscfg = $location.tostring() + "\AccidentalFish.Operations.Cloud\ServiceConfiguration.Cloud.cscfg"
$localcscfg = $location.tostring() + "\AccidentalFish.Operations.Cloud\ServiceConfiguration.Local.cscfg"
$consoleappconfig = $location.tostring() + "\AccidentalFish.Operations.Diagnostics.Console\app.config"

Import-Module .\AccidentalFish.ApplicationSupport.Powershell\bin\debug\AccidentalFish.ApplicationSupport.Powershell.dll

# New-Database -Configuration $configuration -Settings $settings
Set-ApplicationConfiguration -Configuration $configuration -Target $webconfig -Settings $settings
Set-ApplicationConfiguration -Configuration $configuration -Target $csdef -Settings $settings
Set-ApplicationConfiguration -Configuration $configuration -Target $cloudcscfg -Settings $settings
Set-ApplicationConfiguration -Configuration $configuration -Target $localcscfg -Settings $settings
Set-ApplicationConfiguration -Configuration $configuration -Target $consoleappconfig -Settings $settings

New-ApplicationResources -Configuration $configuration -Settings $settings
