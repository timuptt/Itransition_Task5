FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 44301
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FakeUserData.Web/FakeUserData.Web.csproj", "FakeUserData.Web/"]
COPY ["FakeUserData.ApplicationCore/FakeUserData.ApplicationCore.csproj", "FakeUserData.ApplicationCore/"]
RUN dotnet restore "FakeUserData.Web/FakeUserData.Web.csproj"
COPY . .
WORKDIR "/src/FakeUserData.Web"
RUN dotnet build "FakeUserData.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FakeUserData.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FakeUserData.Web.dll"]
