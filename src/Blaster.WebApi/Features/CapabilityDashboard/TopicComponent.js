import Vue from "vue";

const TopicComponent = Vue.component("topic", {
    props: ["topic"],
    data: function() {
        return {
            showData: false,
            showMessageContract: false,
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
            return this.topic.isPrivate ? "green" : "red";
        },
        getPublicText: function () {
            return this.topic.isPrivate ? "✔" : "✖";
        },
        toggleShowAddMessageContract: function() {
            this.showMessageContract = this.showMessageContract ? false : true;
        },
    },
    computed: {

    },
    template: `
        <div class="topic">
            <message-contract-add :enable="showMessageContract" v-on:messagecontractadd-close="toggleShowAddMessageContract()"></message-contract-add>
            <h2 class="title" title="Click to expand" v-on:click="toggleShowData()" >{{ topic.name }}</h2>
            <div class="details" v-if="showData">
                <span class="entry"><span class="entry-title">Private:</span> <div :class="this.getPublicStyling()">{{ this.getPublicText() }}</div></span>
                <span class="entry"><span class="entry-title">Description:</span> <p>{{ topic.description }}</p></span>
                <span class="subtitle">"Message contracts":</span>

                <div class="buttons is-right">
                    <button
                        type="button"
                        v-on:click="toggleShowAddMessageContract()"
                        class="button is-small is-primary">
                        Add Message contract
                    </button>   
                </div>

                <div v-for="message_contract in messages" :key="message_contract.id" class="message-contract" style="margin-bottom: 40px;">
                    <div class="block">
                        <p style="word-wrap: break-word; width: 25%;"><span class="entry-title">Title:</span> {{ message_contract.title }}</p>
                        <div class="schema" style="width: 75%;"><p style="word-wrap: break-word;">{{ message_contract.payload }}</p></div>
                    </div>

                    <div class="block">
                        <div class="buttons is-right" style="width: 100%;">
                            <button
                                type="button"
                                v-on:click="$emit('messagecontractedit-close', message_contract)"
                                class="button is-small is-primary">
                                Edit
                            </button>   
                        </div>                        
                    </div>
                    
                </div>          
            </div>
        </div>
    `
})

export default TopicComponent;
export {TopicComponent};