import Vue from "vue";
import Editor from "./team-editor";
import TeamService from "./teamservice";
import AlertDialog from "./alert-dialog";
import jq from "jquery";
import { currentUser } from "userservice";

const teamService = new TeamService();

export default new Vue({
    el: "#teams-app",
    data: {
        items: [],
        initializing: true,
        currentUser: currentUser
    },
    computed: {
        hasTeams: function () {
            return this.items.length > 0;
        }
    },
    methods: {
        newTeam: function() {
            const editor = Editor.open({
                template: document.getElementById("editor-template"),
                data: {
                    name: "",
                    department: ""
                },
                onClose: () => editor.close(),
                onSave: (teamData) => { 
                    teamService
                        .add(teamData)
                        .then(team => this.items.push(team))
                        .then(() => editor.close())
                        .catch(err => {
                            if (err.status != 200) {
                                const dialog = AlertDialog.open({
                                    template: document.getElementById("error-dialog-template"),
                                    container: jq(".dialog-container", editor.element),
                                    data: {
                                        title: "Error!",
                                        message: `Unable to save team. Server returned (${err.status}) ${err.statusText}.`
                                    }
                                });

                                setTimeout(function() {
                                    dialog.close();
                                }, 3000);
                            }
                        });
                }
            });
        },
        joinTeam: function(teamId) {
            const team = this.items.find(x => x.id == teamId);
            
            teamService
                .join(team.id)
                .then(newMember => {
                    team.members.push(newMember);
                })
                .catch(err => {
                    console.log("error joining team: " + JSON.stringify(err));
                });
        },
        isCurrentUser: function(teamMember) {
            return this.currentUser.id == teamMember.id;
        },
        isCurrentlyMemberOf: function(team) {
            const members = team.members || [];
            return members
                .filter(member => member.id == this.currentUser.id)
                .length > 0;
        }
    }
});