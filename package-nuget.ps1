rmdir nuget\lib -Recurse
rmdir nuget\tools -Recurse

mkdir nuget\lib
mkdir nuget\lib\net45
cp AccidentalFish.ApplicationSupport.Core\bin\Debug\AccidentalFish.ApplicationSupport.Core.dll nuget\lib\net45
cp AccidentalFish.ApplicationSupport.Core\bin\Debug\AccidentalFish.ApplicationSupport.Core.pdb nuget\lib\net45
cp AccidentalFish.ApplicationSupport.Azure\bin\Debug\AccidentalFish.ApplicationSupport.Azure.dll nuget\lib\net45
cp AccidentalFish.ApplicationSupport.Azure\bin\Debug\AccidentalFish.ApplicationSupport.Azure.pdb nuget\lib\net45
cp AccidentalFish.ApplicationSupport.Processes\bin\Debug\AccidentalFish.ApplicationSupport.Processes.dll nuget\lib\net45
cp AccidentalFish.ApplicationSupport.Processes\bin\Debug\AccidentalFish.ApplicationSupport.Processes.pdb nuget\lib\net45

mkdir nuget\tools
mkdir nuget\tools\net45
cp AccidentalFish.ApplicationSupport.Powershell\bin\Debug\*.dll nuget\tools\net45
cp package-tools-init.ps1 nuget\tools\init.ps1

.\.nuget\NuGet.exe pack .\nuget\AccidentalFish.ApplicationSupport.nuspec -NoPackageAnalysis -Symbols
cp *.nupkg ..\LocalRepos