# Script to generate a release zip file
# Need the exe and the Data/Save files in the same folder
# Make sure app.config is not debug

param(
     [Parameter()]
     [string]$Version = "TestRelease"
 )

# Build Solution
dotnet build

# Copy all to a release folder
mkdir Release
Copy-Item -Path "bin\Release\net6.0\*" -Destination "Release"
Copy-Item -Path "Data" -Destination "Release" -Recurse

# Compress and rename
Compress-Archive -Path "Release\*" -DestinationPath "$Version.zip"

# Clear temp file
Remove-Item -Path "Release\*" -Recurse