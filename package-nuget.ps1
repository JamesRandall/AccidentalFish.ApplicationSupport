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
cp .\Source\AccidentalFish.ApplicationSupport.Powershell\bin\Debug\*.dll nuget-powershell\tools\net45
cp package-tools-init.ps1 nuget-powershell\tools\init.ps1
cp .\Source\AccidentalFish.ApplicationSupport.Powershell\AccidentalFish.ApplicationSupport.Powershell.nuspec nuget-powershell
.\nuget pack .\nuget-powershell\AccidentalFish.ApplicationSupport.Powershell.nuspec

.\nuget pack .\Source\AccidentalFish.ApplicationSupport.DependencyResolver\AccidentalFish.ApplicationSupport.DependencyResolver.csproj
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Core\AccidentalFish.ApplicationSupport.Core.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Azure\AccidentalFish.ApplicationSupport.Azure.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Repository.EntityFramework\AccidentalFish.ApplicationSupport.Repository.EntityFramework.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Email.Amazon\AccidentalFish.ApplicationSupport.Email.Amazon.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Email.SendGrid\AccidentalFish.ApplicationSupport.Email.SendGrid.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Unity\AccidentalFish.ApplicationSupport.Unity.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Processes\AccidentalFish.ApplicationSupport.Processes.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Logging.ApplicationInsights\AccidentalFish.ApplicationSupport.Logging.ApplicationInsights.csproj -IncludeReferencedProjects
.\nuget pack .\Source\AccidentalFish.ApplicationSupport.Ninject\AccidentalFish.ApplicationSupport.Ninject.csproj -IncludeReferencedProjects

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
