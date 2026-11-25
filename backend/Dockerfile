FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Bookstore.Api/Bookstore.Api.csproj", "src/Bookstore.Api/"]
COPY ["src/Bookstore.Data/Bookstore.Data.csproj", "src/Bookstore.Data/"]

RUN dotnet restore "src/Bookstore.Api/Bookstore.Api.csproj"

COPY . .
WORKDIR "/src/src/Bookstore.Api"
RUN dotnet publish "Bookstore.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# USER app
RUN mkdir -p /app/uploads && chown -R 1654:1654 /app/uploads
USER 1654:1654

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bookstore.Api.dll"]
