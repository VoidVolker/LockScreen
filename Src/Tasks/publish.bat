dotnet clean
dotnet restore

@ECHO OFF
REM Splitted build of all dependencies - sometimes build not working :/
REM 'Platform' flag is required for preprocessor constants: by default publish doesn't set this constant and #if XXX doesn't work
@ECHO ON

dotnet publish Lib --configuration Release --runtime win-arm64 --property:Platform=arm64 --property:PublishProfile=arm64.pubxml
dotnet publish Lib --configuration Release --runtime win-x64 --property:Platform=x64 --property:PublishProfile=x64.pubxml
dotnet publish Lib --configuration Release --runtime win-x86 --property:Platform=x86 --property:PublishProfile=x86.pubxml

dotnet publish Wallpaper --configuration Release --property:Platform=arm64 --property:PublishProfile=arm64.pubxml
dotnet publish Wallpaper --configuration Release --property:Platform=x64 --property:PublishProfile=x64.pubxml
dotnet publish Wallpaper --configuration Release --property:Platform=x86 --property:PublishProfile=x86.pubxml

dotnet publish LockScreen --configuration Release --property:Platform=arm64 --property:PublishProfile=arm64.pubxml
dotnet publish LockScreen --configuration Release --property:Platform=x64 --property:PublishProfile=x64.pubxml
dotnet publish LockScreen --configuration Release --property:Platform=x86 --property:PublishProfile=x86.pubxml
