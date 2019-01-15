const express = require("express");
const app = express();
const port = 50801;

app.use(express.json());

app.get("/", (req, res) => {
    console.log("Headers: " + JSON.stringify(req.headers));
    // console.log("Body: " + JSON.stringify(req.body));

    res.send("OK");
});

app.listen(port, () => {
    console.log(`Sink is running on port ${port}`);
});