# Use the official .NET SDK 9.0 image for building the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the project files
COPY TimberTrack-API.csproj ./
RUN dotnet restore

# Copy everything and build the app
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the lightweight .NET Runtime for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the build output to the runtime image
COPY --from=build /out ./

# Expose the port 
EXPOSE 3000

# Run the application
CMD ["dotnet", "TimberTrack-API.dll"]
