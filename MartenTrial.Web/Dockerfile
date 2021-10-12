FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MartenTrial.Web/MartenTrial.Web.csproj", "MartenTrial.Web/"]
RUN dotnet restore "MartenTrial.Web/MartenTrial.Web.csproj"
COPY . .
WORKDIR "/src/MartenTrial.Web"
RUN dotnet build "MartenTrial.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MartenTrial.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MartenTrial.Web.dll"]
