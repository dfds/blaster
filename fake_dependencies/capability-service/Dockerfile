FROM node:alpine

WORKDIR /app
COPY . .

RUN npm install
ENV DEBUG=express:*

ENTRYPOINT [ "npm", "run", "start" ]