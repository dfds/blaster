FROM node:15

WORKDIR app

COPY ./src/package.json ./
COPY ./src/webpack.config.js ./
COPY ./src/.babelrc ./

RUN npm install --no-progress --ignore-optional

CMD npm start