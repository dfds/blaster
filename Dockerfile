FROM microsoft/dotnet:2.1.1-aspnetcore-runtime-stretch-slim

WORKDIR /app
COPY /src/Blaster.WebApi/bin/Debug/netcoreapp2.1/* ./

ENTRYPOINT [ "dotnet", "Blaster.WebApi.dll" ]