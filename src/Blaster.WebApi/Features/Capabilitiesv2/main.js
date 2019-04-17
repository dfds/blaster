import Vue from "vue";
import CapabilityService from "./capabilityservice";
import AlertDialog from "./alert-dialog";
import ModelEditor from "modeleditor";
import jq from "jquery";
import { currentUser } from "userservice";

const capabilityService = new CapabilityService();

const app = new Vue({
    el: "#capabilities-app",
    data: {
        items: [],
        membershipRequests: [],
        initializing: true,
        currentUser: currentUser
    },
    computed: {
        hasCapabilities: function () {
            return this.items.length > 0;
        }
    },
    filters: {
        toawspermspage: function(value) {
            return `/awspermissions?capability=${value}`
        }
    },    
    methods: {
        newCapability: function() {
            const editor = ModelEditor.open({
                template: document.getElementById("editor-template"),
                data: {
                    name: "",
                },
                onClose: () => editor.close(),
                onSave: (capabilityData) => { 
                    return capabilityService.add(capabilityData)
                        .then(capability => this.items.push(capability))
                        .then(() => editor.close())
                        .catch(err => {                            
                            if (err.status == 400) {
                                const dialog = AlertDialog.open({
                                    template: document.getElementById("error-dialog-template"),
                                    container: jq(".dialog-container", editor.element),
                                    data: {
                                        title: "Validation issue",
                                        message: err.responseJSON.message
                                    }
                                });

                                setTimeout(function() {
                                    dialog.close();
                                }, 15000);
                            }
                            else if (err.status != 200) {
                                const dialog = AlertDialog.open({
                                    template: document.getElementById("error-dialog-template"),
                                    container: jq(".dialog-container", editor.element),
                                    data: {
                                        title: "Error!",
                                        message: `Unable to save capability. Server returned (${err.status}) ${err.statusText}.`
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
        joinCapability: function(capabilityId) {
            const capability = this.items.find(capability => capability.id == capabilityId);
            this.membershipRequests.push(capability.id);

            capabilityService.join(capability.id)
                .then(() => capability.members.push({ email: this.currentUser.email }))
                .catch(err => console.log("error joining capability: " + JSON.stringify(err)))
                .then(() => {
                        this.membershipRequests = this.membershipRequests.filter(requestedCapabilityId => requestedCapabilityId != capability.id);
                });
        },
        leaveCapability: function(capabilityId) {
            const capability = this.items.find(capability => capability.id == capabilityId);
            const currentUserEmail = this.currentUser.email;

            const editor = ModelEditor.open({
                template: document.getElementById("leave-dialog-template"),
                data: {
                    capabilityName: capability.name
                },
                onClose: () => editor.close(),
                onSave: () => {
                    return capabilityService.leave(capability.id)
                        .then(() => {
                            capability.members = capability.members.filter(member => member.email != currentUserEmail);
                            editor.close();
                        })
                        .catch(err => {
                            console.log("ERROR leaving capability: " + JSON.stringify(err));
                            editor.showError({
                                title: "Error!",
                                message: `Could not leave capability. Try again or reload the page.`
                            });
                        });
                }
            });
        },
        isCurrentUser: function(memberEmail) {
            return this.currentUser.email == memberEmail;
        },
        getMembershipStatusFor: function(capabilityId) {
            const capability = this.items.find(capability => capability.id == capabilityId);
            const isRequested = this.membershipRequests.indexOf(capability.id) > -1;

            if (isRequested) {
                return "requested";
            }

            return this._isCurrentlyMemberOf(capability)
                ? "member"
                : "notmember";
        },
        _isCurrentlyMemberOf: function(capability) {
            if (!capability) {
                return false;
            }

            const members = capability.members || [];
            return members
                .filter(member => member.email == this.currentUser.email)
                .length > 0;
        }
    },
    mounted: function () {
        jq.ready
            .then(() => capabilityService.getAll())
            .then(capabilities => capabilities.forEach(capability => this.items.push(capability)))
            .catch(info => {
                if (info.status != 200) {
                    AlertDialog.open({
                        template: document.getElementById("error-dialog-template"),
                        container: document.getElementById("global-dialog-container"),
                        data: {
                            title: "Error!",
                            message: `Could not retrieve list of capabilities. Server returned (${info.status}) ${info.statusText}.`
                        }
                    });
                }
            })
            .done(() => this.initializing = false);
    }
});