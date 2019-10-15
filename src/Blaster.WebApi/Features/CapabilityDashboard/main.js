import Vue from "vue";
import ModelEditor from "modeleditor";
import CapabilityService from "capabilityservice"
import ConnectionService from "connectionservice";
import ChannelService from "channelservice";
import TopicService from "topicservice"
import jq from "jquery";
import { currentUser } from "userservice";
import AlertDialog from "./alert-dialog";
import FeatureFlag from "featureflag";
import "core-js/features/url-search-params"
import "core-js/stable";
import "regenerator-runtime/runtime";

// Components
import CapabilityEditComponent from "./CapabilityEditComponent";
import CapabilityRetireComponent from "./CapabilityRetireComponent";
import TopicComponent from "./TopicComponent";
import TopicAddComponent from "./TopicAddComponent";
import TopicEditComponent from "./TopicEditComponent";
import MessageContractAddComponent from "./MessageContractAddComponent";
import MessageContractEditComponent from "./MessageContractEditComponent";
import {ChannelPickerComponent, ChannelMinimalComponent, ChannelListComponent} from "../Shared/components/Shared";

const topicService = new TopicService();
const capabilityService = new CapabilityService();
const connectionService = new ConnectionService();
const channelService = new ChannelService();
FeatureFlag.setKeybinding();

Vue.prototype.$featureFlag = new FeatureFlag();

const app = new Vue({
    el: "#capabilitydashboard-app",
    data: {
        capability: null,
        initializing: true,
        currentUser: currentUser,
        membershipRequested: false,
        contextRequested: false,
        topics: null,
        showEditCapability: false,
        showRetireCapability: false,
        showAddTopic: false,
        showEditTopic: false,
        showMessageContractEdit: false,
        messageContractEditData: null,
        topicEditData: null,
        topicsEnabled: false,
        capabilityRetireEnabled: false,
        connections: [],
        communicationConnections: null
    },
    components: {
        'topic': TopicComponent,
        'topic-add': TopicAddComponent,
        'topic-edit': TopicEditComponent,
        'message-contract-add': MessageContractAddComponent,
        'message-contract-edit': MessageContractEditComponent,
        'capability-edit': CapabilityEditComponent,
        'capability-retire': CapabilityRetireComponent,
        'channel-picker': ChannelPickerComponent,
        'channel-minimal': ChannelMinimalComponent,
        'channel-list': ChannelListComponent
    },
    computed: {
        capabilityFound: function() {
            return this.capability != null && this.capability.id != null
        },
        isLegacyComputed: function () { // Determine if Capability is v1 or v2
            const isRootIdEmpty = (this.capability.rootId) ? (this.capability.rootId === "") : true;
            return (this.capability) ? isRootIdEmpty : false; // Ensure capability object exists
        },
        isJoinedComputed: function () {
            var isMemberRawText = this.getMembershipStatusFor();
            return isMemberRawText === "member";
        },
        isAddContextDisallowedComputed: function() {
            var isLegacy = this.isLegacyComputed;
            var isJoined = this.isJoinedComputed;
            return !(isLegacy == false && isJoined == true);
        },
        disabledContextButtonReasonComputed: function() {
            var msg = "";
            if (this.isAddContextDisallowedComputed) {
                msg = "Reason(s) why this button is disabled:" + "\n";
                if (!this.isJoinedComputed) {
                    msg = msg + "You haven't joined this Capability." + "\n";
                }
                if (this.isLegacyComputed) {
                    msg = msg + "This Capability is legacy and won't recieve future updates. Consider creating a new Capability." + "\n";
                }
            }
            return msg;
        },
        channels: function() {
            if (this.connections) {
                return this.connections.map(ch => {
                    return {
                        type: ch.channelType,
                        name: ch.channelName,
                        id: ch.channelId
                    }
                })
            } else {
                return [];
            }
        },
        canLeaveCommunicationChannel: function() {
            if (this.connections) {
                return this.connections.length > 1;
            } else {
                return true;
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
            return this.currentUser.email.toLowerCase() === memberEmail.toLowerCase();
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
                .filter(member => member.email.toLowerCase() == this.currentUser.email.toLowerCase())
                .length > 0;
        },
        toggleShowAddTopic: function() {
            this.showAddTopic = this.showAddTopic ? false : true;
        },
        toggleShowEditCapability: function() {
            if (this.showEditCapability) {
                this.showEditCapability = false;
            } else {
                this.showEditCapability = true;
            }
        },
        toggleShowRetireCapability: function() {
            if (this.showRetireCapability) {
                this.showRetireCapability = false;
            } else {
                this.showRetireCapability = true;
            }
        },        
        toggleShowEditTopic: function(topic) {
            if (this.showEditTopic) {
                this.topicEditData = null;
                this.showEditTopic = false;
            } else {
                this.topicEditData = topic;
                this.showEditTopic = true;
            }
        },
        toggleShowMessageContractEdit: function(data, topicId) {
            if (this.showMessageContractEdit) {
                this.messageContractEditData = null;
                this.showMessageContractEdit = false;
            } else {
                this.messageContractEditData = data;
                this.messageContractEditData.topicId = topicId;
                this.showMessageContractEdit = true;
            }
        },
        handleCapabilityEdit: function(capability) {
            capabilityService.update(this.capability.id, capability)
                .then(() => {
                    return capabilityService.get(this.capability.id);
                })
                .then(data => this.capability = data)
                .catch(err => console.log(JSON.stringify(err)));
            this.toggleShowEditCapability();
        },
        handleCapabilityRetire: function() {
            console.log("Retire Capability clicked.");
            this.toggleShowRetireCapability();
        },
        handleCapabilityJoinChannel: function(channel) {
            connectionService.join({clientId: this.capability.id, clientType: "capability", clientName: this.capability.name, channelId: channel.id, channelName: channel.name, channelType: channel.type})
                .then(() => connectionService.getByCapabilityId(this.capability.id))
                .then(data => this.connections = data)
                .catch(err => console.log(JSON.stringify(err)));
        },
        handleCapabilityLeaveChannel: function(channel) {
            connectionService.leave({clientId: this.capability.id, channelId: channel.id})
            .then(() => connectionService.getByCapabilityId(this.capability.id))
            .then(data => this.connections = data)
            .catch(err => console.log(JSON.stringify(err)));
        },
        handleMessageContractEdit: function(type, description, schema, topicId) {
            topicService.addOrUpdateMessageContract(topicId, type, {"description": description, "content": schema})
                .then(() => {
                    return capabilityService.get(this.capability.id);
                })
                .then(data => this.capability = data)
                .catch(err => console.log(JSON.stringify(err)));
            this.toggleShowMessageContractEdit();
        },
        handleMessageContractAdd: function(description, type, schema, topicId) {
            topicService.addOrUpdateMessageContract(topicId, type, {"description": description, "content": schema})
                .then(() => {
                    return capabilityService.get(this.capability.id);
                })
                .then(data => this.capability = data)
                .catch(err => console.log(JSON.stringify(err)));
        },
        handleMessageContractDelete: function(topicId, type) {
            topicService.deleteMessageContract(topicId, type)
                .then(() => {
                    return capabilityService.get(this.capability.id);
                })
                .then(data => this.capability = data)
                .catch(err => console.log(JSON.stringify(err)));
        },
        addTopic: function(name, description, isPrivate) {
            const payload = {name: name, description: description, isPrivate: isPrivate, messageContracts: []}

            // TODO: Rework this to handle errors
            capabilityService.addTopic(payload, this.capability.id)
                .then(data => {
                    return capabilityService.get(this.capability.id);
                })
                .then(data => this.capability = data)
                .catch(err => console.log("Error adding topic: " + JSON.stringify(err)));

            this.showAddTopic = false;
        },
        editTopic: function(name, description, isPrivate, id) {
            const payload = {name: name, description: description, isPrivate: isPrivate};

            topicService.update(id, payload)
                .then(() => {
                    return capabilityService.get(this.capability.id);
                })
                .then(data => this.capability = data)
                .catch(err => console.log(JSON.stringify(err)));
            this.toggleShowEditTopic();
        },
        getAllTopics: function() {
            const topics = topicService.getAll();
            return topics;
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
        this.topicsEnabled = this.$featureFlag.flagExists("topics") ? this.$featureFlag.getFlag("topics").enabled : false;
        this.capabilityRetireEnabled = this.$featureFlag.flagExists("capabilityretire") ? this.$featureFlag.getFlag("capabilityretire").enabled : false;

        // TODO Handle no or empty capabilityId
        jq.ready
            .then(() => capabilityService.get(capabilityIdParam))
            .then(capability => this.capability = capability)
            .then(capability => {
                jq.ready
                    .then(() => connectionService.getByCapabilityId(capability.id))
                    .then((connections) => {
                        this.connections = connections;
                    })
                    .done();
            })
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
