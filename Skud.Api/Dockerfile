#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Skud.Api/Skud.Api.csproj", "Skud.Api/"]
COPY ["Skud.Application/Skud.Application.csproj", "Skud.Application/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Skud.Domain/Skud.Domain.csproj", "Skud.Domain/"]
COPY ["Skud.Infrastructure/Skud.Infrastructure.csproj", "Skud.Infrastructure/"]
RUN dotnet restore "./Skud.Api/./Skud.Api.csproj"
COPY . .
WORKDIR "/src/Skud.Api"
RUN dotnet build "./Skud.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Skud.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Skud.Api.dll"]