FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Weather.Api/Weather.Api.csproj", "src/Weather.Api/"]
COPY ["src/Weather.Notification/Weather.Notification.csproj", "src/Weather.Notification/"]
COPY ["src/Weather.Forecast.Contract/Weather.Forecast.Contract.csproj", "src/Weather.Forecast.Contract/"]
COPY ["src/Weather.SharedKernel/Weather.SharedKernel.csproj", "src/Weather.SharedKernel/"]
COPY ["src/Weather.Forecast/Weather.Forecast.csproj", "src/Weather.Forecast/"]
RUN dotnet restore "src/Weather.Api/Weather.Api.csproj"
COPY . .
WORKDIR "/src/src/Weather.Api"
RUN dotnet build "Weather.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Weather.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Weather.Api.dll"]
