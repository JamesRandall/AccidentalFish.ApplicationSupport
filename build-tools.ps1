$interop = [System.Runtime.InteropServices.RuntimeEnvironment]
$frameworkPath = $interop::GetRuntimeDirectory()
$msbuild = Join-Path $frameworkPath msbuild.exe

# Build and deploy the application support tools
$msdeploypath = (get-childitem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath") + "MSDeploy"
$buildcmd = $msbuild + " AccidentalFish.ApplicationSupport.sln /p:Configuration=Release"
Invoke-Expression $buildcmd

# TODO: Run MSTest and fail on any errors