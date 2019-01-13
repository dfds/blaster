const express = require("express");
const fs = require("fs");
const util = require("util");

const port = process.env.port || 3000;

const app = express();
app.use(express.json());

const readFile = util.promisify(fs.readFile);
const writeFile = util.promisify(fs.writeFile);

app.get("/api/v1/teams", (req, res) => {
    readFile("./data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            res.json({
                items: teams
            });
        });
});

app.post("/api/v1/teams", (req, res) => {
    const newTeam = Object.assign({
        id: new Date().getTime().toString(),
        members: []
    }, req.body);

    readFile("./data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            teams.push(newTeam);
            return teams;
        })
        .then(teams => JSON.stringify(teams, null, 2))
        .then(json => writeFile("./data.json", json))
        .then(() => {
            res.location(`/api/v1/teams/${newTeam.id}`);
            res.status(201).send(newTeam);
        })
        .catch(err => {
            res.status(500).json(err);
        });
});

app.listen(port, () => {
    console.log("Fake team service is listening on port " + port);
});