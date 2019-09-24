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

app.post("/api/v1/channel/leave", (req, res) => {
    const reqPayload = req.body;

    readFile("./connection-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            const relation = data.items.find(rel => 
                new String(rel.clientId).valueOf() === new String(reqPayload.clientId).valueOf()
                &&
                new String(rel.channelId).valueOf() === new String(reqPayload.channelId).valueOf()
                );
            
            if (!relation) {
                return new Promise(resolve => {
                    res
                        .status(422)
                        .send({message: `Capability with id ${reqPayload.clientId} hasn't joined Channel with id ${reqPayload.channelId}`});
                    resolve();
                });
            } else {
                const desiredRelations = data.items.filter(rel => 
                    rel.clientId.valueOf() === relation.clientId.valueOf() 
                    ?
                    rel.channelId.valueOf() !== relation.channelId.valueOf()
                    :
                    true         
                    );
                
                data.items = desiredRelations;

                return Promise.resolve(serialize(data))
                    .then(json => writeFile("./connection-data.json", json))
                    .then(() => console.log(`Capability with id ${reqPayload.clientId} has left Channel with id ${reqPayload.channelId}`))
                    .then(() => res.sendStatus(200));
            }
        })
        .catch(err => {
            console.log("Error: " + err);
            res.status(500).json(err);
        });
});

app.post("/api/v1/channel/join", (req, res) => {
    const reqPayload = req.body;

    readFile("./connection-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            const relation = data.items.find(rel => 
                new String(rel.clientId).valueOf() === new String(reqPayload.clientId).valueOf()
                &&
                new String(rel.channelId).valueOf() === new String(reqPayload.channelId).valueOf()
                );
            
            if (relation) {
                return new Promise(resolve => {
                    res
                        .status(422)
                        .send({message: `Capability with id ${reqPayload.clientId} has already joined Channel with id ${reqPayload.channelId}`});
                    resolve();
                });
            } else {
                const desiredRelations = data.items;
                // Harcoding a channelType of "slack" until a more concrete API contract/implementation has been made.
                desiredRelations.push({clientId: reqPayload.clientId, channelId: reqPayload.channelId, channelName: reqPayload.channelName, channelType: "slack"});

                data.items = desiredRelations;

                return Promise.resolve(serialize(data))
                .then(json => writeFile("./connection-data.json", json))
                .then(() => console.log(`Capability with id ${reqPayload.clientId} has joined Channel with id ${reqPayload.channelId}`))
                .then(() => res.sendStatus(200));
            }
        })
        .catch(err => {
            console.log("Error: " + err);
            res.status(500).json(err);
        });
});

app.get("/api/v1/connections", (req, res) => {
    const queryParamClientType = req.query.clientType;
    const queryParamClientId = req.query.clientId;
    const queryParamChannelType = req.query.channelType;
    const queryParamChannelId = req.query.channelId;


    readFile("./connection-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            // TODO
            if (queryParamClientType) {

            }

            // TODO
            if (queryParamChannelType) {

            }

            if (queryParamClientId) {
                data = data.items.filter(connection => connection.clientId.valueOf() === queryParamClientId.valueOf());
            }            

            if (queryParamChannelId) {
                data = data.items.filter(connection => connection.channelId.valueOf() === queryParamChannelId.valueOf());                
            }

            return data;
        })
        .then(data => res.json({items: data}))
        .catch(err => {
            console.log("Error: " + err);
            res.status(500).json(err);
        });
})

app.listen(port, () => {
    console.log("Fake Harald service is listening on port " + port);
});