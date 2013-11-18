$interop = [System.Runtime.InteropServices.RuntimeEnvironment]
$frameworkPath = $interop::GetRuntimeDirectory()
$msbuild = Join-Path $frameworkPath msbuild.exe
$msdeploypath = (get-childitem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath") + "MSDeploy"

$buildcmd = $msbuild + " AccidentalFish.Operations.Web\AccidentalFish.Operations.Web.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=operations /p:Password=**password**"
Invoke-Expression $buildcmd