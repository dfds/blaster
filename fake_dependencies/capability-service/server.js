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

app.get("/api/v1/capabilities", (req, res) => {
    readFile("./capability-data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            res.json({
                items: teams
            });
        });
});



app.post("/api/v1/capabilities", (req, res) => {   
    const newTeam = Object.assign({
        id: new Date().getTime().toString(),
        members: [],
        contexts: []

    }, req.body);

    const nameValidationMessage = "Name must be a string of length 3 to 21. consisting of only alphanumeric ASCII characters, starting with a capital letter. Hyphens is allowed.";

    if (newTeam.name === "failme") {
        res.status(400).send({message: nameValidationMessage});
        return;
    }

    const nameValidationRegex = new RegExp('^[A-Z][a-zA-Z0-9\\-]{2,20}$');
    if (newTeam.name.match(nameValidationRegex) == null) {
        res.status(400).send({message: nameValidationMessage});
        return;
    }

    if (newTeam.description == null) {
        // Null or undefined
        newTeam.description = "generic description"
    }

    // RootId generation
    newTeam.RootId = (newTeam.name.substring(0, 20) + "-" + uuidv1().substring(0, 8)).toLocaleLowerCase();

    readFile("./capability-data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            teams.push(newTeam);
            return teams;
        })
        .then(teams => JSON.stringify(teams, null, 2))
        .then(json => writeFile("./capability-data.json", json))
        .then(() => {
            res.location(`/api/v1/capabilities/${newTeam.id}`);
            res.status(201).send(newTeam);
        })
        .catch(err => {
            res.status(500).json(err);
        });
});

app.post("/api/v1/capabilities/:teamid/members", (req, res) => {
    const teamid = req.params.teamid;
    const { email } = req.body;

    readFile("./capability-data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            const team = teams.find(team => team.id == teamid);
            
            if (!team) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Team with id ${teamid} could not be found`});
                    resolve();
                });
            } else {
                team.members.push({ email: email });
                
                return Promise.resolve(serialize(teams))
                    .then(json => writeFile("./capability-data.json", json))
                    .then(() => console.log(`Added member ${email} to team ${team.name}`))
                    .then(() => res.sendStatus(200));
            }
        });
});

app.post("/api/v1/capabilities/:capabilityid/contexts", (req, res) => {
    const capabilityid = req.params.capabilityid;
    const newContext = Object.assign({
        id: new Date().getTime().toString(),
    }, req.body);

    readFile("./capability-data.json")
        .then(data => JSON.parse(data))
        .then(capabilities => {
            const capability = capabilities.find(capability => capability.id == capabilityid);
            
            if (!capability) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Capability with id ${capabilityid} could not be found`});
                    resolve();
                });
            } else {
                capability.contexts.push(newContext);
                return Promise.resolve(serialize(capabilities))
                    .then(json => writeFile("./capability-data.json", json))
                    .then(() => console.log(`Added context ${newContext.name} to capability ${capability.name}`))
                    .then(() => res.sendStatus(200));
            }
        });
});

app.delete("/api/v1/capabilities/:teamid/members/:memberemail", (req, res) => {
    const teamId = req.params.teamid;
    const memberEmail = req.params.memberemail;

    readFile("./capability-data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            const team = teams.find(team => team.id == teamId);
            if (!team) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Team with id ${teamId} could not be found`});
                    resolve();
                });
            } else {
                const desiredMembers = team.members
                    .filter(member => member.email.toLowerCase() != memberEmail.toLowerCase());

                team.members = desiredMembers;

                return Promise.resolve(serialize(teams))
                    .then(json => writeFile("./capability-data.json", json))
                    .then(() => console.log(`Removed member ${memberEmail} from team ${team.name}`))
                    .then(() => res.sendStatus(200));
            }
        })
        .catch(err => {
            console.log("ERROR! " + JSON.stringify(err));
        });
});

app.get("/api/v1/capabilities/:capabilityid", (req, res) => {
    const capabilityid = req.params.capabilityid;
    readFile("./capability-data.json")
        .then(data => JSON.parse(data))
        .then(capabilities => {
            const capability = capabilities.find(cap => cap.id === capabilityid)
            if (!capability) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Capability with id: ${capabilityid} could not be found`});
                    resolve();
                });
            } else {
                return capability;
            }
        })
        .then(capability => {
            if (capability.id)
            {
                readFile("./topic-data.json")
                .then(data => JSON.parse(data))
                .then(topics => {
                    const cap_topics = topics.filter(topic => topic.capabilityId === capability.id)
                    capability.topics = cap_topics;
                    return res.json(capability);
                })
            }

        });
});


app.post("/api/v1/capabilities/:capabilityId/topics", (req, res) => {
    const capabilityId = req.params.capabilityId;
    const newTopic = req.body;
    var newTopicInExpectedFormat = {
        "name": newTopic.name,
        "description": newTopic.description,
        "id": new Date().getTime().toString(),
        "capabilityId": capabilityId,
        "isPrivate": newTopic.isPrivate,
        "messageContracts": []
    }

    readFile("./topic-data.json")
    .then(data => JSON.parse(data))
    .then(topics => {
        topics.push(newTopicInExpectedFormat);
        return topics;
    })
    .then(topics => JSON.stringify(topics, null, 2))
    .then(json => writeFile("./topic-data.json", json))
    .then(() => {
        //res.location(`/api/v1/topics/${newTopic.id}`);
        res.status(200).send();
    })
    .catch(err => {
        res.status(500).json(err);
    });
});

app.get("/api/v1/capabilities/:capabilityId/topics", (req, res) => {
    const capabilityId = req.params.capabilityId;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            return data.filter(topic => new String(topic.capabilityId).valueOf() === new String(capabilityId).valueOf());
        })
        .then(topics => {
            res.json({
                items: topics
            });
        });
});


// ENDPOINT: /topics

app.get("/api/v1/topics", (req, res) => {
    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(topics => {
            res.json({
                items: topics
            });
        });
});

app.get("/api/v1/topics/:topicId", (req, res) => {
    const topicId = req.params.topicId;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            return data.filter(topic => new String(topic.id).valueOf() === new String(topicId).valueOf());
        })
        .then(topics => {
            res.json({
                items: topics
            });
        });
});

app.put("/api/v1/topics/:topicId", (req, res) => {
    const topicId = req.params.topicId;
    const topicInput = req.body;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            const topic = data.find(topic => new String(topic.id).valueOf() === new String(topicId).valueOf());
            
            if (!topic) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Topic with id ${topicId} could not be found`});
                    resolve();
                });
            } else {
                topic.name = topicInput.name === undefined ? topic.name : topicInput.name;
                topic.description = topicInput.description === undefined ? topic.description : topicInput.description;
                topic.isPrivate = topicInput.isPrivate === undefined ? topic.isPrivate : topicInput.isPrivate;

                return Promise.resolve(serialize(data))
                .then(json => writeFile("./topic-data.json", json))
                .then(() => console.log(`Added/Updated Topic ${topicInput.name}`))
                .then(() => res.sendStatus(204));
            }
        })
        .catch(err => {
            res.status(500).json(err);
        });
});

app.get("/api/v1/topics/:topicId/messageContracts", (req, res) => {
    const topicId = req.params.topicId;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            return data.filter(topic => new String(topic.id).valueOf() === new String(topicId).valueOf());
        })
        .then(filtered_topics => {
            if (filtered_topics.length > 0) {
                res.status(200).json({"items": filtered_topics[0].messageContracts});
            } else {
                res.status(404).send();
            }
        })
});


// TODO: Made obsolete in recent API contract revision, to be removed.
app.post("/api/v1/topics/:topicId/messageContracts", (req, res) => {
    const topicId = req.params.topicId;
    const newMessageContract = req.body;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            const topic = data.find(topic => new String(topic.id).valueOf() === new String(topicId).valueOf());
            
            if (!topic) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Topic with id ${topicId} could not be found`});
                    resolve();
                });
            } else {
                topic.messageContracts.push(newMessageContract);
                return Promise.resolve(serialize(data))
                .then(json => writeFile("./topic-data.json", json))
                .then(() => console.log(`Added MessageContract ${newMessageContract.type} to topic ${topic.name}`))
                .then(() => res.sendStatus(200));
            }
        })
        .catch(err => {
            res.status(500).json(err);
        });
});

// TODO: Recent addition, make sure it's available in blaster
app.put("/api/v1/topics/:topicId/messageContracts/:messageContractType", (req, res) => {
    const topicId = req.params.topicId;
    const messageContractType = req.params.messageContractType;
    const newMessageContract = req.body;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            const topic = data.find(topic => new String(topic.id).valueOf() === new String(topicId).valueOf());
            
            if (!topic) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Topic with id ${topicId} could not be found`});
                    resolve();
                });
            } else {
                const mc = topic.messageContracts.find(mc => new String(mc.type).valueOf() === new String(messageContractType).valueOf());

                if (mc) {
                    mc.description = newMessageContract.description === undefined ? mc.description : newMessageContract.description;
                    mc.content = newMessageContract.content === undefined ? mc.content : newMessageContract.content;
                } else {
                    newMessageContract.type = messageContractType;
                    topic.messageContracts.push(newMessageContract);
                }

                return Promise.resolve(serialize(data))
                .then(json => writeFile("./topic-data.json", json))
                .then(() => console.log(`Added/Updated MessageContract ${messageContractType} to topic ${topic.name}`))
                .then(() => res.sendStatus(204));
            }
        })
        .catch(err => {
            res.status(500).json(err);
        });

});

app.delete("/api/v1/topics/:topicId/messageContracts/:messageContractType", (req, res) => {
    const topicId = req.params.topicId;
    const messageContractType = req.params.messageContractType;

    readFile("./topic-data.json")
        .then(data => JSON.parse(data))
        .then(data => {
            const topic = data.find(topic => new String(topic.id).valueOf() === new String(topicId).valueOf());

            if (!topic) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Topic with id ${topicId} could not be found`});
                    resolve();
                });
            } else {
                const desiredMessageContracts = topic.messageContracts.filter(mc => mc.type !== messageContractType);
                topic.messageContracts = desiredMessageContracts;
                return Promise.resolve(serialize(data))
                .then(json => writeFile("./topic-data.json", json))
                .then(() => console.log(`Removed MessageContract ${messageContractType} from topic ${topic.name}`))
                .then(() => res.sendStatus(200));
            }
        })
        .catch(err => {
            console.log("Error: " + err);
            res.status(500).json(err);
        });
});



app.listen(port, () => {
    console.log("Fake team service is listening on port " + port);
});