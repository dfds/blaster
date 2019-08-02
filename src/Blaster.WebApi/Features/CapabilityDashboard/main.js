import Vue from "vue";
import ModelEditor from "modeleditor";
import CapabilityService from "capabilityservice"
import TopicService from "topicservice"
import jq from "jquery";
import { currentUser } from "userservice";
import AlertDialog from "./alert-dialog";
import FeatureFlag from "featureflag";

const topicService = new TopicService();
const capabilityService = new CapabilityService();
FeatureFlag.setKeybinding();

Vue.prototype.$featureFlag = new FeatureFlag();

const TopicComponent = Vue.component("topic", {
    props: ["topic"],
    data: function() {
        return {
            showData: false,
            messages: [
                {
                    "title": "aws_account_added_to_context",
                    "id": 2,
                    "payload": "{\r\n    \"version\": \"1\",\r\n    \"eventName\": \"aws_context_account_created\",\r\n    \"x-correlationId\": \"874ba750-ae81-446f-90c7-01b49228c327\",\r\n    \"x-sender\": \"eg. FQDN of assembly\",\r\n    \"payload\": {\r\n        \"capabilityId\": \"bc3f3bbe-eeee-4230-8b2f-d0e1c327c59c\",\r\n        \"capabilityName\": \"PAX Bookings\",\r\n        \"capabilityRootId\": \"pax-bookings-A43aS\",\r\n        \"contextId\": \"0d03e3ad-2118-46b7-970e-0ca87b59a202\",\r\n        \"contextName\": \"blue\",\r\n        \"accountId\": \"1234567890\",\r\n        \"roleArn\": \"arn:aws:iam::1234567890:role\/pax-bookings-A43aS\",\r\n        \"roleEmail\": \"aws.pax-bookings-a43as@dfds.com\"\r\n    }\r\n}"
                },

                {
                    "title": "context_added_to_capability",
                    "id": 1,
                    "payload": "{\r\n    \"version\": \"1\",\r\n    \"eventName\": \"context_added_to_capability\",\r\n    \"x-correlationId\": \"<guid>|any string\",\r\n    \"x-sender\": \"eg. FQDN of assembly\",\r\n    \"payload\": {\r\n        \"capabilityId\": \"bc3f3bbe-eeee-4230-8b2f-d0e1c327c59c\",\r\n        \"contextId\": \"0d03e3ad-2118-46b7-970e-0ca87b59a202\",\r\n        \"capabilityRootId\": \"pax-bookings-A43aS\",\r\n        \"capabilityName\": \"PAX Bookings\",\r\n        \"contextName\": \"blue\"\r\n    }\r\n}"
                }
            ]
        }
    },
    methods: {
        toggleShowData: function () {
            this.showData = this.showData ? false : true;
        },
        getPublicStyling: function () {
            return this.topic.public ? "green" : "red";
        },
        getPublicText: function () {
            return this.topic.public ? "✔" : "✖";
        }
    },
    computed: {

    },
    template: `
        <div class="topic">
            <h2 class="title" title="Click to expand" v-on:click="toggleShowData()" >{{ topic.name }}</h2>
            <div class="details" v-if="showData">
                <span class="entry"><span class="entry-title">Public:</span> <div :class="this.getPublicStyling()">{{ this.getPublicText() }}</div></span>
                <span class="entry"><span class="entry-title">Description:</span> <p>{{ topic.description }}</p></span>
                <span class="subtitle">"Message contracts":</span>

                <div v-for="message_contract in messages" :key="message_contract.id" class="entry" style="margin-bottom: 40px;">
                    <p style="word-wrap: break-word; width: 25%;"><span class="entry-title">Title:</span> {{ message_contract.title }}</p>
                    <div class="schema" style="width: 75%;"><p style="word-wrap: break-word;">{{ message_contract.payload }}</p></div>
                </div>               
            </div>
        </div>
    `
})

const TopicAddComponent = Vue.component("topic-add", {
    props: ["enable"],
    data: function() {
        return {
            topicName: "",
            topicDescription: "",
            topicPublic: true
        }
    },
    computed: {
        isEnabledStyling: function() {
            return this.enable;
        }
    },
    methods: {
        disable: function() {
            this.enable = false;
        }
    },
    updated: function() {
        if (!this.enable) {
            this.topicName = "";
            this.topicDescription = "";
            this.topicPublic = false;
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('addtopic-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Add Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('addtopic-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <label class="label">Name</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Enter capability name" data-property="name" v-model="topicName">
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">Description</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Description" data-property="description" v-model="topicDescription">
                                </div>
                            </div>
                            <div class="field" style="display: inline-flex;">
                                <label class="label">Public</label>
                                <div class="control" style="margin-left: 6px;">
                                    <input class="checkbox" type="checkbox" placeholder="Public" data-property="Public" v-model="topicPublic">
                                </div>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('addtopic-new-topic', topicName, topicDescription, topicPublic)">Save</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('addtopic-close')">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <button class="modal-close is-large" aria-label="close"></button>
        </div>
    `
})

const app = new Vue({
    el: "#capabilitydashboard-app",
    data: {
        capability: null,
        initializing: true,
        currentUser: currentUser,
        membershipRequested: false,
        contextRequested: false,
        topics: null,
        showAddTopic: false
    },
    components: {
        'topic': TopicComponent,
        'topic-add': TopicAddComponent,
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
                    msg = msg + "This Capability is discontinued. Consider creating a new Capability." + "\n";
                }
            } 
            return msg;
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
        toggleShowAddTopic: function() {
            this.showAddTopic = this.showAddTopic ? false : true;
        },
        addTopic: function(name, description, isPublic) {
            const payload = {name: name, description: description, public: isPublic, capabilityId: this.capability.id}

            // TODO: Rework this to handle errors
            topicService.add(payload)
                .then(data => {
                    console.log(data);
                    this.topics.push(data);
                });

            this.showAddTopic = false;
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
        // TODO Handle no or empty capabilityId
        jq.ready
            .then(() => capabilityService.get(capabilityIdParam))
            .then(capability => this.capability = capability)
            .then(() => topicService.getByCapabilityId(this.capability.id))
            .then(topics => this.topics = topics)
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