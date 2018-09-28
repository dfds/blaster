import jq from "jquery";
import Vue from "vue";
import Editor from "./team-editor";
import TeamService from "./teamservice";
import "./styles"

const teamService = new TeamService();

const app = new Vue({
    el: "#teams-app",
    data: {
        items: []
    },
    methods: {
        newTeam: function() {
            const item = {
                name: "",
                department: ""
            };

            const editor = new Editor();
            editor
                .open(item)
                .then(data => teamService.add(data))
                .then(data => {
                    app.items.push({
                        id: data.id,
                        name: data.name,
                        department: data.department,
                        members: data.members
                    });
                })
                .catch(info => {
                    console.log("ERROR: " + JSON.stringify(info));
                });
        }
    }
});

jq.ready
    .then(() => teamService.getAll())
    .then(data => {
        data.items.forEach(item => app.items.push(item));
    });