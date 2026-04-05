FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Be EXPLICIT about the path from the root
COPY PaymentGateway.API/PaymentGateway.API/PaymentGateway.API.csproj PaymentGateway.API/PaymentGateway.API/
RUN dotnet restore PaymentGateway.API/PaymentGateway.API/PaymentGateway.API.csproj

# Copy EVERYTHING
COPY . .

# Move to the exact folder
WORKDIR "/src/PaymentGateway.API/PaymentGateway.API"

# Force a clean build
RUN dotnet build "PaymentGateway.API.csproj" -c Release
RUN dotnet publish "PaymentGateway.API.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PaymentGateway.API.dll"]