FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

WORKDIR /app
COPY *.csproj ./relay/
RUN cd relay && dotnet restore
COPY * ./relay/
RUN cd relay && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime

run mkdir /config
WORKDIR /app

EXPOSE 80/tcp 5004/tcp 65001/udp

COPY --from=build /app/relay/out ./
ENTRYPOINT ["dotnet", "Relay.dll"]
