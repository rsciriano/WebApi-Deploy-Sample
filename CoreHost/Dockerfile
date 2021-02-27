#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG RUNTIME_VERSION=5.0-buster-slim

FROM mcr.microsoft.com/dotnet/aspnet:$RUNTIME_VERSION AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["CoreHost/CoreHost.csproj", "CoreHost/"]
COPY ["Api/Api.csproj", "Api/"]
COPY ["Aplication/Aplication.csproj", "Aplication/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "CoreHost/CoreHost.csproj"
COPY . .
WORKDIR "/src/CoreHost"
RUN dotnet build "CoreHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHost.dll"]
