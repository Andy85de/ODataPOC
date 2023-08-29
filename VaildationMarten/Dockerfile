FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VaildationMarten/VaildationMarten.csproj", "VaildationMarten/"]
RUN dotnet restore "VaildationMarten/VaildationMarten.csproj"
COPY . .
WORKDIR "/src/VaildationMarten"
RUN dotnet build "VaildationMarten.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VaildationMarten.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VaildationMarten.dll"]
