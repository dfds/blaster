FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim

WORKDIR /app
COPY ./output/app ./

ENTRYPOINT [ "dotnet", "Blaster.WebApi.dll" ]