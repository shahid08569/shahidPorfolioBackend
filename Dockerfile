# Stage 1: Build the .NET solution
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for caching restore layer
COPY ["ShahidPortfolio.slnx", "./"]
COPY ["src/ShahidPortfolio.Domain/ShahidPortfolio.Domain.csproj", "src/ShahidPortfolio.Domain/"]
COPY ["src/ShahidPortfolio.Application/ShahidPortfolio.Application.csproj", "src/ShahidPortfolio.Application/"]
COPY ["src/ShahidPortfolio.Infrastructure/ShahidPortfolio.Infrastructure.csproj", "src/ShahidPortfolio.Infrastructure/"]
COPY ["src/ShahidPortfolio.API/ShahidPortfolio.API.csproj", "src/ShahidPortfolio.API/"]
COPY ["tests/ShahidPortfolio.Tests/ShahidPortfolio.Tests.csproj", "tests/ShahidPortfolio.Tests/"]

RUN dotnet restore ShahidPortfolio.slnx

# Copy all source files and publish
COPY . .
WORKDIR /src/src/ShahidPortfolio.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

# Ensure uploads directory exists
RUN mkdir -p wwwroot/uploads

ENTRYPOINT ["dotnet", "ShahidPortfolio.API.dll"]
