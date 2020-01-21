import Vue from "vue";
import TopicComponent from "./TopicComponent";
import TopicAddComponent from "./TopicAddComponent"

const capabilityTopicsComponent = Vue.component("capabilityTopics", {
	props: ["topicsEnabled", "capabilityId", "isJoinedComputed"],
	data: function () {
		return {
			showAddTopic: false,
			topics: [
				{
					"id": "1a599887-a1c7-49a5-a521-4c415244039a",
					"name": "Capability-A.topic-1bb209c4-f45b-42af-b8f7-2ac43d31a6a4",
					"description": "I won't tell",
					"capabilityId": "0d03e3ad-2118-46b7-970e-0ca87b59a202",
					"partitions": 3
				},
				{
					"id": "6df3f838-fd1c-4f4c-aabc-95c760b07036",
					"name": "Capability-A.topic-a03e58e7-8d68-45cb-a7c8-8c7a2e9b6f58",
					"description": "I won't tell",
					"capabilityId": "0d03e3ad-2118-46b7-970e-0ca87b59a202",
					"partitions": 3
				}
			]
		}
	},
	methods: {
		showCreateTopicFlow: function () {
			this.showAddTopic = this.showAddTopic ? false : true;
		},
		topicCreated(){
			
		}
	},
	components: {
		'topic': TopicComponent,
		'topic-add': TopicAddComponent
	},
	template: `
			<div class="container box" v-if="topicsEnabled">
                <h1 class="title is-uppercase">Topics</h1>

                <div>
                    <p>
                         Listed below is the Kafka topics that are attached to this Capability.
                    </p>
                </div>
                <br>

                <div class="buttons is-right">
                         <button
                            type="button"
                            v-on:click="showCreateTopicFlow()"
                            v-bind:class="{tooltip: !isJoinedComputed, 'is-tooltip-bottom': !isJoinedComputed, 'is-tooltip-multiline': !isJoinedComputed }"
                            :disabled='!isJoinedComputed'
                            data-tooltip="You haven't joined this Capability"
                            class="button is-small is-primary">
                            Add Topic
                        </button>
                </div>

                <topic-add :enable="showAddTopic"  v-on:addtopic-topic-created="topicCreated" v-on:addtopic-close="showCreateTopicFlow()"></topic-add>
                <div class="topics">
                    <div v-for="topic in topics" :key="topic.id">
                        <topic :topic="topic"></topic>
                    </div>
                </div>
            </div>
    `
})

export default capabilityTopicsComponent;
export {capabilityTopicsComponent};