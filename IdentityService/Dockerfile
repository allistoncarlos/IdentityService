FROM microsoft/dotnet:2.1.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1.302-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY IdentityService.csproj IdentityService/
RUN dotnet restore
COPY . .
WORKDIR /src/IdentityService
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityService.dll"]