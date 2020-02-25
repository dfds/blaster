import Vue from "vue";
import TopicComponent from "./TopicComponent";
import TopicAddComponent from "./TopicAddComponent"
import TopicService from "./topicservice";

const capabilityTopicsComponent = Vue.component("capabilityTopics", {
	props: ["topicsEnabled", "capabilityId", "isJoinedComputed"],
	mounted: function () {
		if (this.topicsEnabled) {
			this.loadTopics();
		}
	},
	data: function () {
		return {
			showAddTopic: false,
			topics: []
		}
	},
	methods: {
		showCreateTopicFlow: function () {
			this.showAddTopic = this.showAddTopic ? false : true;
		},
		loadTopics() {
			let topicService = new TopicService();
			topicService.getByCapabilityId(this.capabilityId).then(data => this.topics = data);
		},
		topicCreated() {
			this.showAddTopic = false;
			this.loadTopics();
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
					Listed below is the Kafka topics that are attached to this Capability.<br />
					You are welcome to contact the development excellence department if you need a topic deleted.
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

			<topic-add :enable="showAddTopic" :capability-id="capabilityId" v-on:topicAdded="topicCreated"
					   v-on:addtopic-close="showCreateTopicFlow()"></topic-add>
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