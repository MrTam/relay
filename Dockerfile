FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base
WORKDIR /app
EXPOSE 80/tcp 5004/tcp 65001/udp

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app

FROM build as publish
RUN dotnet publish -c Release -o /app

FROM base as final
WORKDIR /app
COPY --from=publish /app .
RUN ln -sf /app/Config /config

VOLUME /config
ENTRYPOINT ["dotnet", "Relay.dll"]
