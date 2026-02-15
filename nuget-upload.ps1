$nuget_api_key = cat "NUGET_API_KEY"
if (-not $?) { Exit }

$version=Read-Host("version to upload?")

Write-Host $("uploading " + $version)
if ($(Read-Host("correct? (y/n)")) -ne "y") {
    Exit
}

dotnet clean
if (-not $?) { Exit }
dotnet build -c release
if (-not $?) { Exit }
dotnet pack
if (-not $?) { Exit }
dotnet test
if (-not $?) { Exit }

dotnet nuget push $("src/SimulationFramework/bin/Release/SimulationFramework." + $version + ".nupkg") --api-key $nuget_api_key --source https://api.nuget.org/v3/index.json
dotnet nuget push $("src/SimulationFramework.OpenGL/bin/Release/SimulationFramework.OpenGL." + $version + ".nupkg") --api-key $nuget_api_key --source https://api.nuget.org/v3/index.json
dotnet nuget push $("src/SimulationFramework.Desktop/bin/Release/SimulationFramework.Desktop." + $version + ".nupkg") --api-key $nuget_api_key --source https://api.nuget.org/v3/index.json