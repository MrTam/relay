FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

WORKDIR /app
COPY *.csproj ./relay/
RUN cd relay && dotnet restore
COPY * ./relay/
RUN cd relay && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime

WORKDIR /app
COPY --from=build /app/relay/out ./
ENTRYPOINT ["dotnet", "Relay.dll"]
