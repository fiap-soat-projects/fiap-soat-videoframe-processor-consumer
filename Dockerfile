FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Drivers/Consumer/Consumer.csproj", "src/Drivers/Consumer/"]
COPY ["src/Adapter/Adapter.csproj", "src/Adapter/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Drivers/Infrastructure/Infrastructure.csproj", "src/Drivers/Infrastructure/"]
RUN dotnet restore "./src/Drivers/Consumer/Consumer.csproj"
COPY . .
WORKDIR "/src/src/Drivers/Consumer"
RUN dotnet build "./Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.dll"]