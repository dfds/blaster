const express = require("express");
const fs = require("fs");
const util = require("util");

const port = process.env.port || 3000;

const app = express();
app.use(express.json());

const readFile = util.promisify(fs.readFile);
const writeFile = util.promisify(fs.writeFile);
const serialize = (data) => JSON.stringify(data, null, 2);
const deserialize = (text) => JSON.parse(text);

app.get("/api/topics", (req, res) => {
    readFile("./data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            res.json({
                items: teams
            });
        });
});

app.post("/api/topics", (req, res) => {
    const { name } = req.body;

    readFile("./data.json")
        .then(data => JSON.parse(data))
        .then(topics => {
            topics.push(name);
            return topics;
        })
        .then(topics => JSON.stringify(topics, null, 2))
        .then(json => writeFile("./data.json", json))
        .then(() => {
            res.sendStatus(204);
        })
        .catch(err => {
            res.status(500).json(err);
        });
});

app.listen(port, () => {
    console.log("Fake tika is listening on port " + port);
});