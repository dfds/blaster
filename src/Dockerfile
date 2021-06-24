FROM node:14

WORKDIR app

COPY ./src/package.json ./
COPY ./src/webpack.config.js ./
COPY ./src/.babelrc ./

RUN npm install --no-progress --ignore-optional

CMD npm start