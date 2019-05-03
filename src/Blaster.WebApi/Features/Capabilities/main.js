import Vue from "vue";
import CapabilityService from "capabilityservice"
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
    methods: {
        newCapability: function() {
            const editor = ModelEditor.open({
                template: document.getElementById("editor-template"),
                data: {
                    name: "",
                    description: "",
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
    filters: {
        capabilitydetails: function(capabilityId) {
            return `/capabilitydashboard?capabilityId=${capabilityId}`
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