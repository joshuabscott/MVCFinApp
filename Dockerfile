
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["MVCFinApp.csproj", ""]
RUN dotnet restore "./MVCFinApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MVCFinApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVCFinApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MVCFinApp.dll