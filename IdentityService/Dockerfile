FROM microsoft/dotnet:2.1-sdk  AS builder
WORKDIR /sln/IdentityService
COPY . .

RUN dotnet restore IdentityService.csproj
RUN dotnet build -c Release --no-restore

COPY . .
RUN dotnet publish --output /sln/ --configuration Release

FROM microsoft/dotnet:2.1-sdk 
WORKDIR /sln
COPY --from=builder /sln .
ENTRYPOINT ["dotnet", "IdentityService.dll"]]