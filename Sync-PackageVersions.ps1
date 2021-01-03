[xml] $centralPackageVersions = Get-Content ".\Packages.props"
[xml] $webProjectPackages = Get-Content ".\WebHost\packages.config"


$packageReferences = $centralPackageVersions.Project.ItemGroup.PackageReference

foreach ($package in $webProjectPackages.packages.package) {

    $packageReference =  $packageReferences | Where-Object Update -eq $package.id

    if ($packageReference) {

        if ($packageReference.Version -match "^\[+(.+?)\]*$") {
            
            $updateVersion = $Matches[1]   

            if ($updateVersion -ne $package.version ) {

                Write-Output "Updating $($packageReference.Update) from $($package.version) to $updateVersion"
                nuget.exe update ".\WebHost\WebHost.csproj" -Id $packageReference.Update -Version $updateVersion -Verbosity normal
            }
            else {
                Write-Output "$($packageReference.Update) v$updateVersion package already syncronized "
            }


        }
        else {
            Write-Error "Invalid package version $($packageReference.Update) $($packageReference.Version)"
        }


    }
    
    
}

