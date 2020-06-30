FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 9000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Xortd/Xortd.csproj", "Xortd/"]
RUN dotnet restore "Xortd/Xortd.csproj"
COPY . .
WORKDIR "/src/Xortd"
RUN dotnet build "Xortd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xortd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xortd.dll"]