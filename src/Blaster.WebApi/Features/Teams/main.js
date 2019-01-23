import Vue from "vue";
import Editor from "./team-editor";
import TeamService from "./teamservice";
import AlertDialog from "./alert-dialog";
import ModelEditor from "modeleditor";
import jq from "jquery";
import { currentUser } from "userservice";
import "./styles"

const teamService = new TeamService();

const app = new Vue({
    el: "#teams-app",
    data: {
        items: [],
        membershipRequests: [],
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
            const team = this.items.find(team => team.id == teamId);
            this.membershipRequests.push(team.id);

            teamService.join(team.id)
                .then(() => team.members.push({ email: this.currentUser.email }))
                .catch(err => console.log("error joining team: " + JSON.stringify(err)))
                .done(() => {
                        this.membershipRequests = this.membershipRequests.filter(requestedTeamId => requestedTeamId != team.id);
                });
        },
        leaveTeam: function(teamId) {
            const team = this.items.find(aTeam => aTeam.id == teamId);
            const currentUserEmail = this.currentUser.email;

            const editor = ModelEditor.open({
                template: document.getElementById("leave-dialog-template"),
                data: {
                    teamName: team.name
                },
                onClose: () => editor.close(),
                onSave: () => {
                    return teamService.leave(team.id)
                        .then(() => {
                            team.members = team.members.filter(member => member.email != currentUserEmail);
                            editor.close();
                        })
                        .catch(err => {
                            console.log("ERROR leaving team: " + JSON.stringify(err));
                            editor.showError({
                                title: "Error!",
                                message: `Could not leave team. Try again or reload the page.`
                            });
                        });
                }
            });
        },
        isCurrentUser: function(memberEmail) {
            return this.currentUser.email == memberEmail;
        },
        getMembershipStatusFor: function(teamId) {
            const team = this.items.find(team => team.id == teamId);
            const isRequested = this.membershipRequests.indexOf(team.id) > -1;

            if (isRequested) {
                return "requested";
            }

            return this._isCurrentlyMemberOf(team)
                ? "member"
                : "notmember";
        },
        _isCurrentlyMemberOf: function(team) {
            if (!team) {
                return false;
            }

            const members = team.members || [];
            return members
                .filter(member => member.email == this.currentUser.email)
                .length > 0;
        }
    },
    mounted: function () {
        jq.ready
            .then(() => teamService.getAll())
            .then(teams => teams.forEach(team => this.items.push(team)))
            .catch(info => {
                if (info.status != 200) {
                    AlertDialog.open({
                        template: document.getElementById("error-dialog-template"),
                        container: document.getElementById("global-dialog-container"),
                        data: {
                            title: "Error!",
                            message: `Could not retrieve list of teams. Server returned (${info.status}) ${info.statusText}.`
                        }
                    });
                }
            })
            .done(() => this.initializing = false);
    }
});