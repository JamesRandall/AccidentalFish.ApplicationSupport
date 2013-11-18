$location = Get-Location
$configuration = $location.tostring() + "\configuration.xml"
$settings = $location.tostring() + "\settings-cloud.xml"
.\deploy.ps1

#.\deploy-website.ps1