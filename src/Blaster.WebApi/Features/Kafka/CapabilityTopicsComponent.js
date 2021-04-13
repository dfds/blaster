import Vue from "vue";
import TopicComponent from "./TopicComponent";
import TopicAddComponent from "./TopicAddComponent"
import TopicService from "./topicservice";

const capabilityTopicsComponent = Vue.component("capabilityTopics", {
	props: ["capabilityId", "isJoinedComputed"],
	mounted: function () {
      this.loadClusters()
      .then(() => {
        return this.loadTopics();
      })
      .then(() => {
        this.topicsViewData = this.getTopicsByCluster();
      })
      .then(() => {
        var onlyClustersInUse = [];
        this.clusters.forEach(cluster => {
          if (this.topicsViewData.has(cluster.id)) {
            onlyClustersInUse.push(cluster);
          }
        });

        this.clusters = onlyClustersInUse;
      });
	},
	data: function () {
		return {
			showAddTopic: false,
			topics: [],
      clusters: [],
      topicsViewData: new Map(),
      abandonedTopics: []
		}
	},
	methods: {
		showCreateTopicFlow: function () {
			this.showAddTopic = this.showAddTopic ? false : true;
		},
		loadTopics() {
			let topicService = new TopicService();
			return topicService.getByCapabilityId(this.capabilityId).then(data => this.topics = data);
		},
    loadClusters() {
			let topicService = new TopicService();
      return topicService.getClusters().then(data => this.clusters = data);
    },
    getTopicsByCluster() {
      var sorted = new Map();

      this.clusters.forEach(cluster => {
        sorted.set(cluster.id, []);
      });

      this.topics.forEach(topic => {
        if (sorted.has(topic.kafkaClusterId)) {
          sorted.get(topic.kafkaClusterId).push(topic);
        } else
        {
          this.abandonedTopics.push(topic);
        }
      });

      // Check if "sorted" has a cluster listed with no topics, if so, remove it from viewing.
      for (let [key, value] of sorted.entries()) {
        if (value.length === 0) {
          sorted.delete(key);
        }
      }

      return sorted;
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
		<div class="container box">
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

			<topic-add :enable="showAddTopic" :clusters="clusters" :capability-id="capabilityId" v-on:topicAdded="topicCreated"
					   v-on:addtopic-close="showCreateTopicFlow()"></topic-add>
			<div class="topics">
        <div v-for="cluster in clusters" :key="cluster.id">
          {{cluster.name}} <span v-if="cluster.clusterId !== undefined">({{cluster.clusterId}})</span>

          <div v-for="topic in topicsViewData.get(cluster.id)" :key="topic.id">
					  <topic :topic="topic"></topic>
				  </div>

        </div>
        <div>
          Abandoned (no matching Cluster)

          <div v-for="topic in abandonedTopics" :key="topic.id">
            <topic :topic="topic" :abandoned="true"></topic>
          </div>                  
        </div>
			</div>
		</div>
	`
})

export default capabilityTopicsComponent;
export {capabilityTopicsComponent};