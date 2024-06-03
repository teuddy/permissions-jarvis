FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Permissions.Api/Permissions.Api.csproj", "Permissions.Api/"]
RUN dotnet restore "Permissions.Api/Permissions.Api.csproj"
COPY . .
WORKDIR "/src/Permissions.Api"
RUN dotnet build "Permissions.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Permissions.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Permissions.Api.dll"]
