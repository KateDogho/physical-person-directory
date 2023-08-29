FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Your_Strong_Password

# Run the SQL Server entrypoint script
CMD /bin/bash ./entrypoint.sh

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PhysicalPersonDirectory.Api.csproj", "PhysicalPersonDirectory.Api/"]
RUN dotnet restore "PhysicalPersonDirectory.Api/PhysicalPersonDirectory.Api.csproj"
COPY . .

WORKDIR /app/build
RUN chmod +x /app/build

WORKDIR "/src/PhysicalPersonDirectory.Api"

RUN dotnet build "PhysicalPersonDirectory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhysicalPersonDirectory.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhysicalPersonDirectory.Api.dll"]
