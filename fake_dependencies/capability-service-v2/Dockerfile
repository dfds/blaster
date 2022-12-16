FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

ENV DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:5051

EXPOSE 5051

ENTRYPOINT [ "dotnet" ]
CMD [ "run", "--no-launch-profile" ]