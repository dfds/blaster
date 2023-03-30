import Vue from "vue";
import TopicComponent from "./TopicComponent";
import TopicAddComponent from "./TopicAddComponent"
import TopicService from "./topicservice";

const capabilityTopicsComponent = Vue.component("capabilityTopics", {
	props: ["capabilityId", "isJoinedComputed"],
	mounted: function () {
      this.loadAll();
	  setInterval(() => this.loadAll(), 5*1000);
	},
	data: function () {
		return {
			showAddTopic: false,
			topics: [],
      clusters: [],
      clustersViewData: [],
      topicsViewData: new Map(),
      abandonedTopics: []
		}
	},
	methods: {
		showCreateTopicFlow: function () {
			this.showAddTopic = this.showAddTopic ? false : true;
		},
		loadAll() {
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
	  
			  this.clustersViewData = onlyClustersInUse;
			});
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
				<strong>We have moved!</strong> Topics are now available as part of the v2 experience at <a href="https://build.dfds.cloud/v2">https://build.dfds.cloud/v2</a>
			</div>

			<br />
		</div>
	`
})

export default capabilityTopicsComponent;
export {capabilityTopicsComponent};