# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SkyTunesCsharp.csproj", "./"]
RUN dotnet restore "SkyTunesCsharp.csproj"
COPY . .
RUN dotnet build "SkyTunesCsharp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SkyTunesCsharp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkyTunesCsharp.dll"]