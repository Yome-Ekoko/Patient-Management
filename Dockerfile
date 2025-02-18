# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Patient_Management/Patient_Management.csproj", "Patient_Management/"]
COPY ["Patient_Management.Infrastructure/Patient_Management.Infrastructure.csproj", "Patient_Management.Infrastructure/"]
COPY ["Patient_Management.Core/Patient_Management.Core.csproj", "Patient_Management.Core/"]
COPY ["Patient_Management.Persistence/Patient_Management.Persistence.csproj", "Patient_Management.Persistence/"]
COPY ["Patient_Management.Domain/Patient_Management.Domain.csproj", "Patient_Management.Domain/"]
RUN dotnet restore "./Patient_Management/Patient_Management.csproj"
COPY . .
WORKDIR "/src/Patient_Management"
RUN dotnet build "./Patient_Management.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Patient_Management.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patient_Management.dll"]