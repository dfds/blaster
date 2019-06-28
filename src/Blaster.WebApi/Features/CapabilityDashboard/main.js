import Vue from "vue";
import ModelEditor from "modeleditor";
import CapabilityService from "capabilityservice"
import TopicService from "topicservice"
import jq from "jquery";
import { currentUser } from "userservice";
import AlertDialog from "./alert-dialog";

const topicService = new TopicService();
const capabilityService = new CapabilityService();

const app = new Vue({
    el: "#capabilitydashboard-app",
    data: {
        capability: null,
        initializing: true,
        currentUser: currentUser,
        membershipRequested: false,
        contextRequested: false
    },
    computed: {
        capabilityFound: function() {
            return this.capability != null && this.capability.id != null
        },
        isLegacyComputed: function () { // Determine if Capability is v1 or v2
            if (this.capability) // Ensure capability object exists
            {
                if (this.capability.rootId) { // Check if rootId is null
                    if (this.capability.rootId === "") {
                        return true;
                    } else {
                        return false;
                    }
                }
                return true;
            }
        },
        isJoinedComputed: function () {
            var isMemberRawText = this.getMembershipStatusFor();
            return isMemberRawText === "member";
        },
        isAddContextDisallowedComputed: function() {
            var isLegacy = this.isLegacyComputed;
            var isJoined = this.isJoinedComputed;

            if (isLegacy == false && isJoined == true) {
                return false;
            } else {
                return true;
            }
        },
        disabledContextButtonReasonComputed: function() {
            if (this.isAddContextDisallowedComputed) {
                var msg = "Reason(s) why this button is disabled:" + "\n";
                if (!this.isJoinedComputed) {
                    msg = msg + "You haven't joined this Capability." + "\n";
                }
                if (this.isLegacyComputed) {
                    msg = msg + "This Capability is discontinued. Consider creating a new Capability." + "\n";
                }
                return msg;
            } else {
                return "";
            }
        }
    },
    filters: {
        topicdetails: function(topicName){
            return `/Topicdetails?topicname=${topicName}`
        }
    },
    methods: {
        isCurrentUser: function(memberEmail) {
            return this.currentUser.email === memberEmail;
        },
        hasMembers: function(){
          const cap = this.capability;
          
          if (cap == null || cap.members == null || cap.members.length < 1)
              return false;
          
          return true;
        },
        getMembershipStatusFor: function() {
            const isRequested = this.membershipRequested;
            if (isRequested) {
                return "requested";
            }
            return this._isCurrentlyMemberOf(this.capability)
                ? "member"
                : "notmember";
        },
        getContextStatusFor: function(){
            const isContextRequested = this.contextRequested;
            if(isContextRequested)
            {
                return "requested"
            }
            const contexts = this.capability.contexts || [];
            return contexts.length > 0
                ? "added"
                : "notadded";
        },
        _isCurrentlyMemberOf: function(capability) {
            if (!capability) {
                return false;
            }
            const members = capability.members || [];
            return members
                .filter(member => member.email == this.currentUser.email)
                .length > 0;
        },
        joinCapability: function() {
            this.membershipRequested = true;
            capabilityService.join(this.capability.id)
                .then(() => this.capability.members.push({ email: this.currentUser.email }))
                .catch(err => console.log("error joining capability: " + JSON.stringify(err)))
                .then(() => {
                        this.membershipRequested = false;
                });
        },
        leaveCapability: function() {
            const capabilityId = this.capability.id;
            const capabilityName = this.capability.name;
            const currentUserEmail = this.currentUser.email;

            const editor = ModelEditor.open({
                template: document.getElementById("leave-dialog-template"),
                data: {
                    capabilityName: capabilityName
                },
                onClose: () => editor.close(),
                onSave: () => {
                    return capabilityService.leave(capabilityId)
                        .then(() => {
                            this.capability.members = this.capability.members.filter(member => member.email != currentUserEmail);
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
        addContext: function() {
            this.contextRequested = true;
            capabilityService.addContext(this.capability.id)
                .catch(err => console.log("error requesting context: " + JSON.stringify(err)))
                .then(() => this.capability.contexts.push({name: "default"}))
                .then(() => {
                        this.contextRequested = false;
                });
            const editor = ModelEditor.open({
                template: document.getElementById("add-context-dialog-template"),
                data: {
                },
                onClose: () => editor.close(),
            });
        },
        newTopic: function() {
            const editor= ModelEditor.open({
                template: document.getElementById("new-topic-template"),
                data: {
                    name: "",
                    description: "",
                    visibility: ""
                },
                onClose: () => editor.close(),
                onSave: (newTopic) => {
                    topicService.add(newTopic)
                    .then(() => {capabilityService.addTopic(this.capability.id, newTopic.name)})
                    .then(() => this.capability.topics.push({name: newTopic.name}))
                    .then(() => editor.close());
                }
            });
        }
    },
    mounted: function () {
        const capabilityIdParam = new URLSearchParams(window.location.search).get('capabilityId');
        // TODO Handle no or empty capabilityId
        jq.ready
            .then(() => capabilityService.get(capabilityIdParam))
            .then(capability => this.capability = capability)
            .catch(info => {
                if (info.status != 200) {
                    AlertDialog.open({
                        template: document.getElementById("error-dialog-template"),
                        container: document.getElementById("global-dialog-container"),
                        data: {
                            title: "Error!",
                            message: `Could not retrieve capability. Server returned (${info.status}) ${info.statusText}.`
                        }
                    });
                }
            })
            .done(() => this.initializing = false);
    }
});