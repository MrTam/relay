FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /app
COPY *.csproj ./relay/
RUN cd relay && dotnet restore
COPY relay/. ./relay/

WORKDIR /app/relay
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime

WORKDIR /app
COPY --from=build /app/relay/out ./
ENTRYPOINT ["dotnet", "relay.dll"]