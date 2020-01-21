import Vue from "vue";

const TopicComponent = Vue.component("topic", {
	props: ["topic", "commonprefix"],
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
		forwardNewMessageContract: function(description, type, schema, topicId) {
			this.$emit("messagecontractadd-new", description, type, schema, topicId);
			this.toggleShowAddMessageContract();
		},
		topicName: function(commonPrefix, topicMisc) {
			var name = "";
			name = commonPrefix;

			if (topicMisc !== "") {
				name = name + "." + topicMisc;
			}

			return name;
		},
	},
	computed: {

	},
	template: `
        <div class="topic">
            <h2 class="title" title="Click to expand" v-on:click="toggleShowData()" >{{ topic.name }}</h2>
            <div class="details" v-if="showData">
                <span class="entry"><span class="entry-title">Description:</span> <p>{{ topic.description }}</p></span>
				<span class="entry"><span class="entry-title">Partitions:</span> <p>{{ topic.partitions }}</p></span>

			</div>
        </div>
    `
})

export default TopicComponent;
export {TopicComponent};