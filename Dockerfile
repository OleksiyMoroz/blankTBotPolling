FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
COPY /src /app
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV BOT_TOKEN 612253730:AAGbODLCLTrZKP7-fUR3LHvpHHEgP9ynKvM
ENTRYPOINT ["dotnet", "emptyBotPolling.dll"]