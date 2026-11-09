FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_EnableDiagnostics=0

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["FunEvents.Server/FunEvents.Server.csproj", "FunEvents.Server/"]
COPY ["FunEvents.Application/FunEvents.Application.csproj", "FunEvents.Application/"]
COPY ["FunEvents.Domain/FunEvents.Domain.csproj", "FunEvents.Domain/"]
RUN dotnet restore "FunEvents.Server/FunEvents.Server.csproj"
COPY . .
WORKDIR /src/FunEvents.Server

RUN dotnet build "FunEvents.Server.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "FunEvents.Server.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FunEvents.Server.dll"]
