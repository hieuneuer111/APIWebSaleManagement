# ----- STAGE 1: BUILD -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và khôi phục dependency
COPY ["WebAPISalesManagement/WebAPISalesManagement.csproj", "WebAPISalesManagement/"]
RUN dotnet restore "WebAPISalesManagement/WebAPISalesManagement.csproj"

# Copy toàn bộ source
COPY . .

WORKDIR "/src/WebAPISalesManagement"
RUN dotnet build "WebAPISalesManagement.csproj" -c Release -o /app/build

# ----- STAGE 2: PUBLISH -----
FROM build AS publish
RUN dotnet publish "WebAPISalesManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ----- STAGE 3: RUNTIME -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ✅ Cài chứng chỉ gốc CA để tránh lỗi trên Android
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebAPISalesManagement.dll"]
