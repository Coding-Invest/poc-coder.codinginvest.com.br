FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
USER root

RUN apt-get update \
    && apt-get install -y git ca-certificates curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g @angular/cli \
    && rm -rf /var/lib/apt/lists/*

COPY ["./Server/server.pfx", "/app/server.pfx"]
COPY ["./Server/server.crt", "/usr/local/share/ca-certificates/server.crt"]
RUN update-ca-certificates

USER app
WORKDIR /app
EXPOSE 8081
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Server/Server.csproj", "Server/"]
COPY ["../Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./Server/Server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Server/Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server/Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
RUN mkdir -p /app/repos && chmod 777 /app/repos
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]