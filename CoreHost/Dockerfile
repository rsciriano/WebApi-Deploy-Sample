#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["global.json", "."] #
COPY ["Packages.props", "."] #
COPY ["Directory.Build.targets", "."] #
COPY ["CoreHost/CoreHost.csproj", "CoreHost/"]
COPY ["Api/Api.csproj", "Api/"]
COPY ["Aplication/Aplication.csproj", "Aplication/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain.UnitTests/Domain.UnitTests.csproj", "Domain.UnitTests/"]
COPY ["Api.IntegrationTests/Api.IntegrationTests.csproj", "Api.IntegrationTests/"]

RUN dotnet restore "CoreHost/CoreHost.csproj" -v d
RUN dotnet restore "Domain.UnitTests/Domain.UnitTests.csproj" -v d
RUN dotnet restore "Api.IntegrationTests/Api.IntegrationTests.csproj" -v d

COPY . .
WORKDIR "/src/CoreHost"
RUN dotnet build "CoreHost.csproj" -c Release -o /app/build

FROM build AS tests
WORKDIR "/src/Domain.UnitTests"
RUN dotnet build "Domain.UnitTests.csproj" -c Release #-o /app/tests
RUN dotnet test --logger:trx

FROM build AS integrationtestrunner
WORKDIR "/src/Api.IntegrationTests/"
RUN dotnet build "Api.IntegrationTests.csproj" -c Release #-o /app/tests
CMD ["dotnet", "test", "--logger:trx"]


FROM build AS publish
RUN dotnet publish "CoreHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHost.dll"]
