#Setup build image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

#Set container filesystem to /build (and create folder if it doesnt exist)
WORKDIR /build

# install dependencies 
RUN apt-get update && apt-get install -y apt-utils && apt-get install -y curl

# get node.js install script and pass it to execute
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash

# install node 
RUN apt-get install nodejs && node -v

# npm installs automatically 
RUN npm -v

#Copy files to container file system.
COPY ./src/package.json ./src/package.json
COPY ./src/webpack.config.js ./src/webpack.config.js
COPY ./src/Blaster.WebApi ./src/Blaster.WebApi

WORKDIR /build/src

#Build nmp dependencies
RUN npm install && npm run-script build

#Set workdir to current project folder
WORKDIR /build/src/Blaster.WebApi

#Restore csproj packages.
RUN dotnet restore

#Compile source code using standard Release profile
RUN dotnet publish -c Release -o /build/out

#Setup final container images.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

#Copy binaries from publish container to final container
COPY --from=build-env /build/src/Blaster.WebApi/wwwroot ./wwwroot
COPY --from=build-env /build/out .

RUN curl -o /tmp/rds-combined-ca-bundle.pem https://s3.amazonaws.com/rds-downloads/rds-combined-ca-bundle.pem \
    && mv /tmp/rds-combined-ca-bundle.pem /usr/local/share/ca-certificates/rds-combined-ca-bundle.crt \
    && update-ca-certificates
	
# SSL
ENV CAPABILITY_SERVICE_KAFKA_SSL_CA_LOCATION=/app/cert.pem

RUN curl -sS -o /app/cert.pem https://curl.haxx.se/ca/cacert.pem

#Run dotnet executable
ENTRYPOINT ["dotnet", "Blaster.WebApi.dll"]