FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Navigator.TeamsBot/NaviBot.csproj", "src/Navigator.TeamsBot/"]
COPY ["src/NaviBot.Data/NaviBot.Data.csproj", "src/NaviBot.Data/"]
COPY ["src/NaviBot.Services/NaviBot.Services.csproj", "src/NaviBot.Services/"]
RUN dotnet restore "src/Navigator.TeamsBot/NaviBot.csproj"
COPY . .
WORKDIR "/src/src/Navigator.TeamsBot"
RUN dotnet build "NaviBot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NaviBot.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NaviBot.dll"]
