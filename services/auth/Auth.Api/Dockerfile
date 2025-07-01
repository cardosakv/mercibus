FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Auth.Api/Auth.Api.csproj", "Auth.Api/"]
COPY ["Auth.Infrastructure/Auth.Infrastructure.csproj", "Auth.Infrastructure/"]
COPY ["Auth.Application/Auth.Application.csproj", "Auth.Application/"]
COPY ["Auth.Domain/Auth.Domain.csproj", "Auth.Domain/"]
RUN dotnet restore "Auth.Api/Auth.Api.csproj"
COPY . .
WORKDIR "/src/Auth.Api"
RUN dotnet build "./Auth.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Api.dll"]
