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

app.get("/api/v1/capabilities", (req, res) => {
    readFile("./data.json")
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

    if (newTeam.name === "failme") {
        res.status(400).send({message: "Name must be a string of length 3 to 32. consisting of only alphanumeric ASCII characters, starting with a capital letter. Underscores and hyphens are allowed."});
        return;
    }

    if (newTeam.description == null) {
        // Null or undefined
        newTeam.description = "generic description"
    }

    readFile("./data.json")
        .then(data => JSON.parse(data))
        .then(teams => {
            teams.push(newTeam);
            return teams;
        })
        .then(teams => JSON.stringify(teams, null, 2))
        .then(json => writeFile("./data.json", json))
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

    readFile("./data.json")
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
                    .then(json => writeFile("./data.json", json))
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

    readFile("./data.json")
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
                    .then(json => writeFile("./data.json", json))
                    .then(() => console.log(`Added context ${newContext.name} to capability ${capability.name}`))
                    .then(() => res.sendStatus(200));
            }
        });
});

app.delete("/api/v1/capabilities/:teamid/members/:memberemail", (req, res) => {
    const teamId = req.params.teamid;
    const memberEmail = req.params.memberemail;

    readFile("./data.json")
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
                    .then(json => writeFile("./data.json", json))
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
    readFile("./data.json")
        .then(data => JSON.parse(data))
        .then(capabilities => {
            const capability = capabilities.find(cap => cap.id === capabilityid)
            if (!capability) {
                return new Promise(resolve => {
                    res
                        .status(404)
                        .send({message: `Capability with id ${capabilityid} could not be found`});
                    resolve();
                });
            } else {
                return res.json(capability);
            }
        });
});

app.listen(port, () => {
    console.log("Fake team service is listening on port " + port);
});