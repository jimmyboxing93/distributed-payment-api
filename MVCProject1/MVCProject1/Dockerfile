# Stage 1: Build the app using the full SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish "MVCProject1.csproj" -c Release -o out

# Stage 2: Runtime - Create the final slim image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MVCProject1.dll"]