FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PhysicalPersonDirectory.Api/PhysicalPersonDirectory.Api.csproj", "PhysicalPersonDirectory.Api/"]
RUN dotnet restore "PhysicalPersonDirectory.Api/PhysicalPersonDirectory.Api.csproj"
COPY . .
WORKDIR "/src/PhysicalPersonDirectory.Api"
RUN dotnet build "PhysicalPersonDirectory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhysicalPersonDirectory.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhysicalPersonDirectory.Api.dll"]
