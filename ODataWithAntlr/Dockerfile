FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ODataPOC/ODataPOC.csproj", "ODataPOC/"]
RUN dotnet restore "ODataPOC/ODataPOC.csproj"
COPY . .
WORKDIR "/src/ODataPOC"
RUN dotnet build "ODataPOC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ODataPOC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ODataPOC.dll"]
