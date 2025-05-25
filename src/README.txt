1. Install .NET SDK https://dotnet.microsoft.com/en-us/download

2. open appsettings.json and put your telegram bot toke to BOT_TOKEN key value pair 
3. Run command line commands one by one in src folder:
 - dotnet restore
 - dotnet publish -c Release
4. Run to bin/Release/net7.0/publish/ and run emptyBotPolling.exe