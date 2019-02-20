const express = require("express");
const fs = require("fs");
const util = require("util");
const path = require("path");

const port = process.env.port || 3000;

const app = express();

app.get("/api/configs/kubeconfig", (req, res) => {
    const configPath = path.resolve(__dirname, './config');

    res.setHeader('Content-Type', 'text/yaml');
    res.setHeader('Content-Disposition', "attachment; filename=config")

    res.sendFile(configPath);
});

app.listen(port, () => {
    console.log("Fake iamrole service is listening on port " + port);
});