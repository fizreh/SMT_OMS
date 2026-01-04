# Stage 1: Build & Test
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and csproj files
COPY *.sln .
COPY API/*.csproj ./API/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Tests/*.csproj ./Tests/

# Restore all projects
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build backend in Release
RUN dotnet build API/API.csproj -c Release -o /app/build

# Run tests (fail build if tests fail)
RUN dotnet test Tests/Tests.csproj --logger:trx

# Publish backend for runtime
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create folder for SQLite database and Logs
RUN mkdir -p /app/data /app/logs \
    && chmod -R 777 /app/data /app/logs #Persissions set

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose port your API will run on
ENV ASPNETCORE_URLS=http://+:7121
EXPOSE 7121

ENTRYPOINT ["dotnet", "API.dll"]