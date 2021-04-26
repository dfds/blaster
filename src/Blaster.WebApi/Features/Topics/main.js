import Vue from "vue";
import jq from "jquery";
import "core-js/stable";
import "regenerator-runtime/runtime";
import FeatureFlag from "featureflag";


import TopicService from "topicservice";
import CapabilityService from "capabilityservice";

import {ServerTable, ClientTable, Event} from 'vue-tables-2';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';

import {isIE, BannerComponent} from "../Shared/components/Shared";

FeatureFlag.setKeybinding();

Vue.use(ClientTable);
Vue.component('Datatable', DataTable);
Vue.component('Column', Column);
Vue.component('inputtext', InputText);

const topicService = new TopicService(Vue.prototype.$http, Vue.prototype.$userService);
const capabilityService = new CapabilityService(Vue.prototype.$http, Vue.prototype.$userService);

const app = new Vue({
    el: "#topic-app",
    components: {
        'banner': BannerComponent
    },
    computed: {
        showIEBanner: function() {
            return isIE();
        }
    },
    data: {
      tablesData: {
        columns: ['capability', 'topicName', 'clusterId', 'description'],
        data: []
      },
      filters: {},
      password: "",
      capabilityData: [],
      clusters: {}
    },
    methods: {
      getCapabilityById: function(capabilityId) {
        return this.capabilityData.find(cap => capabilityId.valueOf() == cap.id.valueOf());
      },
      getClusterById: function(clusterId) {
        return this.clusters.get(clusterId);
      }
    },
    mounted: function() {
      jq.ready
        .then(() => {
          var caps = capabilityService.getAll();
          return jq.when(caps);
        })
        .then(caps => {
          this.capabilityData = caps.items;
        })
        .then(() => {
          var clusters =topicService.getClusters();

          return jq.when(clusters);
        })
        .then(clusters => {
          var map = new Map();
          clusters.forEach(cluster => {
            map.set(cluster.id, cluster);
          });
          this.clusters = map;
        })
        .then(() => {
          var topics = topicService.getAll();

          return jq.when(topics);
        })
        .then(topics => {
          var payload = [];

          topics.forEach(t => {
            var cap = this.getCapabilityById(t.capabilityId);

            if (cap == undefined) 
            {
              cap = {name: "DELETED", id: "DELETED"}
            }

            var tableTopic = {
              capability: {name: cap.name, id: cap.id},
              description: t.description,
              topicName: t.name,
              clusterId: this.getClusterById(t.kafkaClusterId).clusterId,
              clusterName: this.getClusterById(t.kafkaClusterId).name
            };

            payload.push(tableTopic);
          });

          return payload;
        })
        .then(filteredTopics => {
          // Filter so it only shows public topics
          return filteredTopics.filter(t => {
            var firstFourChars = t.topicName.substring(0, 4);
            return firstFourChars.valueOf() === "pub.";
          });
        })
        .then(filteredTopics => {
          filteredTopics.sort(function(a, b) {
            var name_a = a.topicName.toUpperCase();
            var name_b = b.topicName.toUpperCase();

            return (name_a < name_b) ? -1 : (name_a > name_b) ? 1 : 0;
          })
          this.tablesData.data = filteredTopics;
        });
    }
})

 