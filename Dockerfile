FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["resicare-backend-webapi.csproj", "./"]
RUN dotnet restore "resicare-backend-webapi.csproj"

COPY . .
RUN dotnet publish "resicare-backend-webapi.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production

CMD ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet resicare-backend-webapi.dll
