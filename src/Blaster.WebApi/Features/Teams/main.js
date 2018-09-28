import jq from "jquery";
import TeamService from "./teamservice";
import AlertDialog from "./alert-dialog";
import app from "./app";
import "./styles"

const teamService = new TeamService();

jq.ready
    .then(() => teamService.getAll())
    .then(data => {
        const items = data.items || [];
        items.forEach(item => app.items.push(item));
    })
    .catch(info => {
        if (info.status != 200) {
            const dialog = AlertDialog.open({
                template: document.getElementById("error-dialog-template"),
                container: document.getElementById("global-dialog-container"),
                data: {
                    title: "Error!",
                    message: `Could not retrieve list of teams. Server returned (${info.status}) ${info.statusText}.`
                }
            });
        }
    })
    .done(() => app.initializing = false);