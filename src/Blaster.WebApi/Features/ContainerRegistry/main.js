import app from "./app";
import jq from "jquery";
import { service as repositoryService } from "./containerrepositoryservice";
import AlertDialog from "alertdialog";

jq.ready
    .then(() => repositoryService.getAll())
    .then(repos => app.setRepositories(repos))
    .then(() => app.setStatus("running"))
    .catch(err => {
        app.setStatus("error");

        AlertDialog.open({
            template: document.getElementById("error-dialog-template"),
            container: document.getElementById("global-dialog-container"),
            data: {
                title: "Error!",
                message: `Could not retrieve list of teams. Server returned (${err.status}) ${err.statusText}. Try again by reloading the page.`
            }
        });
    });