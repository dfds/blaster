FROM node:12.13.0-alpine

COPY ./package.json ./package.json
COPY ./webpack.config.js ./webpack.config.js

RUN npm install

CMD ["npm", "run start:dev"]