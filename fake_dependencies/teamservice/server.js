const express = require("express");
const fs = require("fs");

const app = express();
const port = process.env.port || 3000;

app.get("/api/v1/teams", (req, res) => {
    fs.readFile("./data.json", (error, data) => {
        let teams = new Array();
        if (!error) {
            teams = JSON.parse(data);
        }
        res.send({
            items: teams
        });
    });
});

app.listen(port, () => {
    console.log("Fake team service is listening on port " + port);
});