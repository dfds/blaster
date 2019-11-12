FROM node:12.13.0-alpine

COPY ./package.json .
COPY ./webpack.config.js .

RUN npm install --silent

CMD ["npm", "start"]