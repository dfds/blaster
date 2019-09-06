FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim

WORKDIR /app
COPY ./output/app ./

ENTRYPOINT [ "dotnet", "Blaster.WebApi.dll" ]