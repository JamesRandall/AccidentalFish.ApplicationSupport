# Setup a redirect to the required version of Newtonsoft Json
$newtonsoft = [System.Reflection.Assembly]::LoadFrom("$PSScriptRoot\..\..\Source\AccidentalFish.ApplicationSupport.Powershell\bin\debug\Newtonsoft.Json.dll")
$onAssemblyResolveEventHandler = [System.ResolveEventHandler] {
  param($sender, $e)
  # You can make this condition more or less version specific as suits your requirements
  if ($e.Name.StartsWith("Newtonsoft.Json")) {
    return $newtonsoft
  }
  foreach($assembly in [System.AppDomain]::CurrentDomain.GetAssemblies()) {
    if ($assembly.FullName -eq $e.Name) {
      return $assembly
    }
  }
  return $null
}
[System.AppDomain]::CurrentDomain.add_AssemblyResolve($onAssemblyResolveEventHandler)

$location = Get-Location
$configuration = $location.tostring() + "\configuration.xml"
$settings = $location.tostring() + "\settings.xml"
$buildServerSettings = $location.tostring() + "\build-server-settings.xml"
$appconfig = $location.tostring() + ".\app.config"

Get-ChildItem "..\..\Source\AccidentalFish.ApplicationSupport.Powershell\bin\debug\" -Filter *.dll |
Foreach-Object {
	Import-Module $_.FullName
}

Set-ApplicationConfiguration -Configuration $configuration -Target $appconfig -Settings $settings

# Set the key vault secrets from the secrets file
..\..\Source\CmdLine\SetKeyVaultSecrets\bin\Debug\SetKeyVaultSecrets.exe -c $configuration -s $settings -i your-kv-clientid -k your-kv-client-secret -u your-keyvault-url -v

# Create resources using the key vault by supplying the build server settings file (has no info in it) and using the key vault for the secret settings
..\..\Source\CmdLine\NewApplicationResources\bin\Debug\NewApplicationResources.exe -c $configuration -s $buildServerSettings -i your-kv-clientid -k your-kv-client-secret -u your-keyvault-url -v

# Detach the event handler (not detaching can lead to stack overflow issues when closing PS)
[System.AppDomain]::CurrentDomain.remove_AssemblyResolve($onAssemblyResolveEventHandler)
