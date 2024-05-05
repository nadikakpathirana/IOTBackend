#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["IOTBackend.API/IOTBackend.API.csproj", "IOTBackend.API/"]
COPY ["IOTBackend.Application/IOTBackend.Application.csproj", "IOTBackend.Application/"]
COPY ["IOTBackend.Domain/IOTBackend.Domain.csproj", "IOTBackend.Domain/"]
COPY ["IOTBackend.Infrastructure/IOTBackend.Infrastructure.csproj", "IOTBackend.Infrastructure/"]
COPY ["IOTBackend.Persistance/IOTBackend.Persistance.csproj", "IOTBackend.Persistance/"]
COPY ["IOTBackend.Shared/IOTBackend.Shared.csproj", "IOTBackend.Shared/"]

RUN dotnet restore "./IOTBackend.API/IOTBackend.API.csproj"
COPY . .
WORKDIR "/src/IOTBackend.API"
RUN dotnet build "./IOTBackend.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as migrations
RUN dotnet tool install --version 6.0.29 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet ef database update

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IOTBackend.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IOTBackend.API.dll"]
