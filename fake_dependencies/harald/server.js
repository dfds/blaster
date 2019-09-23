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
                new String(rel.senderId).valueOf() === new String(reqPayload.senderId).valueOf()
                &&
                new String(rel.channelId).valueOf() === new String(reqPayload.channelId).valueOf()
                );
            
            if (!relation) {
                return new Promise(resolve => {
                    res
                        .status(422)
                        .send({message: `Capability with id ${reqPayload.senderId} hasn't joined Channel with id ${reqPayload.channelId}`});
                    resolve();
                });
            } else {
                const desiredRelations = data.items.filter(rel => 
                    rel.senderId.valueOf() === relation.senderId.valueOf() 
                    ?
                    rel.channelId.valueOf() !== relation.channelId.valueOf()
                    :
                    true         
                    );
                
                data.items = desiredRelations;

                return Promise.resolve(serialize(data))
                    .then(json => writeFile("./connection-data.json", json))
                    .then(() => console.log(`Capability with id ${reqPayload.senderId} has left Channel with id ${reqPayload.channelId}`))
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
                new String(rel.senderId).valueOf() === new String(reqPayload.senderId).valueOf()
                &&
                new String(rel.channelId).valueOf() === new String(reqPayload.channelId).valueOf()
                );
            
            if (relation) {
                return new Promise(resolve => {
                    res
                        .status(422)
                        .send({message: `Capability with id ${reqPayload.senderId} has already joined Channel with id ${reqPayload.channelId}`});
                    resolve();
                });
            } else {
                const desiredRelations = data.items;
                desiredRelations.push({senderId: reqPayload.senderId, channelId: reqPayload.channelId, channelName: reqPayload.channelName});

                data.items = desiredRelations;

                return Promise.resolve(serialize(data))
                .then(json => writeFile("./connection-data.json", json))
                .then(() => console.log(`Capability with id ${reqPayload.senderId} has joined Channel with id ${reqPayload.channelId}`))
                .then(() => res.sendStatus(200));
            }
        })
        .catch(err => {
            console.log("Error: " + err);
            res.status(500).json(err);
        });
});

app.get("/api/v1/connections", (req, res) => {
    const queryParamSenderType = req.query.senderType;
    const queryParamSenderId = req.query.senderId;
    const queryParamChannelType = req.query.channelType;
    const queryParamChannelId = req.query.channelId;


    readFile("./connection-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            // TODO
            if (queryParamSenderType) {

            }

            // TODO
            if (queryParamChannelType) {

            }

            if (queryParamSenderId) {
                data = data.items.filter(connection => connection.senderId.valueOf() === queryParamSenderId.valueOf());
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