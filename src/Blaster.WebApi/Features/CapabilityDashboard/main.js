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
                    "payload": "{\"version\":\"1\",\"eventName\":\"aws_context_account_created\",\"x-correlationId\":\"874ba750-ae81-446f-90c7-01b49228c327\",\"x-sender\":\"eg. FQDN of assembly\",\"payload\":{\"capabilityId\":\"bc3f3bbe-eeee-4230-8b2f-d0e1c327c59c\",\"capabilityName\":\"PAX Bookings\",\"capabilityRootId\":\"pax-bookings-A43aS\",\"contextId\":\"0d03e3ad-2118-46b7-970e-0ca87b59a202\",\"contextName\":\"blue\",\"accountId\":\"1234567890\",\"roleArn\":\"arn:aws:iam::1234567890:role/pax-bookings-A43aS\",\"roleEmail\":\"aws.pax-bookings-a43as@dfds.com\"}                    }    "
                },

                {
                    "title": "context_added_to_capability",
                    "id": 1,
                    "payload": "{\"version\":\"1\",\"eventName\":\"context_added_to_capability\",\"x-correlationId\":\"<guid>|any string\",\"x-sender\":\"eg. FQDN of assembly\",\"payload\":{\"capabilityId\":\"bc3f3bbe-eeee-4230-8b2f-d0e1c327c59c\",\"contextId\":\"0d03e3ad-2118-46b7-970e-0ca87b59a202\",\"capabilityRootId\":\"pax-bookings-A43aS\",\"capabilityName\":\"PAX Bookings\",\"contextName\":\"blue\"}} "
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

const app = new Vue({
    el: "#capabilitydashboard-app",
    data: {
        capability: null,
        initializing: true,
        currentUser: currentUser,
        membershipRequested: false,
        contextRequested: false,
        topics: null
    },
    components: {
        'topic': TopicComponent
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