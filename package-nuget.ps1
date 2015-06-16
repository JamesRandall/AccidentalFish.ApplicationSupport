Param(
	[switch]$pushLocal,
	[switch]$pushNuget,
	[switch]$cleanup
)

if (Test-Path -Path nuget-powershell) 
{
	rmdir nuget-powershell -Recurse
}
rm *.nupkg

mkdir nuget-powershell
mkdir nuget-powershell\tools
mkdir nuget-powershell\tools\net45
cp AccidentalFish.ApplicationSupport.Powershell\bin\Debug\*.dll nuget-powershell\tools\net45
cp package-tools-init.ps1 nuget-powershell\tools\init.ps1
cp .\AccidentalFish.ApplicationSupport.Powershell\AccidentalFish.ApplicationSupport.Powershell.nuspec nuget-powershell
.\nuget pack .\nuget-powershell\AccidentalFish.ApplicationSupport.Powershell.nuspec

.\nuget pack .\AccidentalFish.ApplicationSupport.Core\AccidentalFish.ApplicationSupport.Injection.csproj
.\nuget pack .\AccidentalFish.ApplicationSupport.Core\AccidentalFish.ApplicationSupport.Core.csproj
.\nuget pack .\AccidentalFish.ApplicationSupport.Azure\AccidentalFish.ApplicationSupport.Azure.csproj -IncludeReferencedProjects
.\nuget pack .\AccidentalFish.ApplicationSupport.Repository.EntityFramework\AccidentalFish.ApplicationSupport.Repository.EntityFramework.csproj -IncludeReferencedProjects
.\nuget pack .\AccidentalFish.ApplicationSupport.Email.Amazon\AccidentalFish.ApplicationSupport.Email.Amazon.csproj -IncludeReferencedProjects
.\nuget pack .\AccidentalFish.ApplicationSupport.Email.SendGrid\AccidentalFish.ApplicationSupport.Email.SendGrid.csproj -IncludeReferencedProjects
.\nuget pack .\AccidentalFish.ApplicationSupport.Unity\AccidentalFish.ApplicationSupport.Unity.csproj -IncludeReferencedProjects
.\nuget pack .\AccidentalFish.ApplicationSupport.Processes\AccidentalFish.ApplicationSupport.Processes.csproj -IncludeReferencedProjects

if ($pushLocal)
{
	cp *.nupkg \LocalPackageRepository
}

if ($pushNuget)
{
	.\nuget push *.nupkg
}

if ($cleanup)
{
	rmdir nuget-powershell -Recurse
	rm *.nupkg
}
