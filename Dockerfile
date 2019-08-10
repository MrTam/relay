FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /src

RUN apk add nodejs npm

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine
WORKDIR /app
EXPOSE 80/tcp 5004/tcp 65001/udp
COPY --from=build /app .
RUN ln -sf /app/Config /config

VOLUME /config
ENTRYPOINT ["dotnet", "Relay.dll"]
