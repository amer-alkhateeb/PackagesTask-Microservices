# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Delivery/DeliveryService.API/DeliveryService.API.csproj", "src/Services/Delivery/DeliveryService.API/"]
COPY ["src/Services/Delivery/DeliveryService.Application/DeliveryService.Application.csproj", "src/Services/Delivery/DeliveryService.Application/"]
COPY ["src/Services/Delivery/DeliveryService.Domain/DeliveryService.Domain.csproj", "src/Services/Delivery/DeliveryService.Domain/"]
COPY ["src/Services/Delivery/DeliveryService.Infrastructure/DeliveryService.Infrastructure.csproj", "src/Services/Delivery/DeliveryService.Infrastructure/"]
RUN dotnet restore "./src/Services/Delivery/DeliveryService.API/DeliveryService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Delivery/DeliveryService.API"
RUN dotnet build "./DeliveryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeliveryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryService.API.dll"]