Set-Variable output -option Constant -value "nupkgs"
Set-Variable source -option Constant -value "C:\NuGet\packages"

# AspNetCore
Invoke-Expression "dotnet pack --include-symbols .\Sources\Plea.AspNetCore\Plea.AspNetCore.csproj -p:PackageVersion=$($args[0]).$($args[1]).$($args[2]) --output $output"
Invoke-Expression "nuget add .\nupkgs\Plea.AspNetCore.$($args[0]).$($args[1]).$($args[2]).nupkg -source $source"

# Core
Invoke-Expression "dotnet pack --include-symbols .\Sources\Plea.Core\Plea.Core.csproj -p:PackageVersion=$($args[0]).$($args[1]).$($args[2]) --output $output"
Invoke-Expression "nuget add .\nupkgs\Plea.Core.$($args[0]).$($args[1]).$($args[2]).nupkg -source $source"