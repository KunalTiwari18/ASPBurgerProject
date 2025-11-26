# Dockerfile for BBURGERClone (ASP.NET Core 7)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
# don't hardcode ports here; we'll set ASPNETCORE_URLS at runtime via the entrypoint script
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV NUGET_XMLDOC_MODE=skip

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BBURGERClone.csproj", "./"]
RUN dotnet restore "BBURGERClone.csproj"
COPY . .
WORKDIR /src
RUN dotnet publish "BBURGERClone.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# copy entrypoint script
COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh

# expose the default port (convention) -- Render will tell us what PORT to use at runtime
EXPOSE 8080

ENTRYPOINT ["./entrypoint.sh"]
