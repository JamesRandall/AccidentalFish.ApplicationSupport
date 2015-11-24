$location = Get-Location
$configuration = $location.tostring() + "\configuration.xml"
$settings = $location.tostring() + "\settings.xml"

$appconfig = $location.tostring() + ".\app.config"

Import-Module ..\..\Source\AccidentalFish.ApplicationSupport.Powershell\bin\debug\AccidentalFish.ApplicationSupport.Powershell.dll

Set-ApplicationConfiguration -Configuration $configuration -Target $appconfig -Settings $settings -CheckForMissingSettings $TRUE

New-ApplicationResources -Configuration $configuration -Settings $settings
