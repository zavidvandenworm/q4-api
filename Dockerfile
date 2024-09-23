# Use the official Microsoft image as a base for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /source

# Copy the .csproj files for each project and restore the dependencies
COPY q4-api/q4-api.csproj ./q4-api/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Application/Application.csproj ./Application/

# Restore dependencies for each project
RUN dotnet restore ./q4-api/q4-api.csproj

# Copy the remaining source code
COPY . .

# Build the solution
RUN dotnet build ./q4-api/q4-api.csproj -c Release -o /app/build

# Publish the app (compiling it for production)
RUN dotnet publish ./q4-api/q4-api.csproj -c Release -o /app/publish

# Use the runtime image for final deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build stage to the runtime stage
COPY --from=build /app/publish .

# Expose the port on which the API will run
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "q4-api.dll"]
