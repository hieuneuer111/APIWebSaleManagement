# === STAGE 1: BUILD ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore packages
COPY WebAPISalesManagement.csproj ./
RUN dotnet restore WebAPISalesManagement.csproj

# Copy toàn bộ source và publish
COPY . ./
RUN dotnet publish WebAPISalesManagement.csproj -c Release -o /app/publish

# === STAGE 2: RUNTIME ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# ✅ Thêm ca-certificates để tránh lỗi SSL trên Android
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "WebAPISalesManagement.dll"]
