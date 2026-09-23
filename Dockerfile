### Build for Mac ARM:
### docker buildx build --platform linux/arm64 -t pratico-api:1.0.0 --load .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Pratico.Api/Pratico.Api.csproj", "Pratico.Api/"]
COPY ["Pratico.Business/Pratico.Business.csproj", "Pratico.Business/"]
COPY ["Pratico.Data/Pratico.Data.csproj", "Pratico.Data/"]
COPY ["Pratico.Dominio/Pratico.Dominio.csproj", "Pratico.Dominio/"]
COPY ["Pratico.Worker.OutBox/Pratico.Worker.OutBox.csproj", "Pratico.Worker.OutBox/"]

RUN dotnet restore "Pratico.Api/Pratico.Api.csproj"

COPY . .
WORKDIR /src/Pratico.Api
RUN dotnet publish "Pratico.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Pratico.Api.dll"]
