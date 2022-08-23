Set-Variable output -option Constant -value "nupkgs"
Set-Variable source -option Constant -value "C:\NuGet\packages"

# Core
Invoke-Expression "dotnet pack .\Sources\Plea.Core\Plea.Core.csproj -p:PackageVersion=$($args[0]).$($args[1]).$($args[2])$($args[3]) --version-suffix $($args[3]) --output $output"
Invoke-Expression "nuget add .\nupkgs\Plea.Core.$($args[0]).$($args[1]).$($args[2])$($args[3]).nupkg -source $source"