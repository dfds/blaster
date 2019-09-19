const express = require("express");
const fs = require("fs");
const util = require("util");
const uuidv1 = require('uuid/v1');

const port = process.env.port || 3000;

const app = express();
app.use(express.json());

const readFile = util.promisify(fs.readFile);
const writeFile = util.promisify(fs.writeFile);
const serialize = (data) => JSON.stringify(data, null, 2);
const deserialize = (text) => JSON.parse(text);

app.get("/api/v1/channels", (req, res) => {
    readFile("./channels-data.json")
        .then(data => res.send(data));
});

app.listen(port, () => {
    console.log("Fake Harald service is listening on port " + port);
});