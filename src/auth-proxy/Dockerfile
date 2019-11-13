FROM node:alpine

WORKDIR /app
COPY ./src/auth-proxy .

RUN npm install

ENTRYPOINT [ "npm", "start" ]