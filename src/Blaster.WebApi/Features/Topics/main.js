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
        columns: ['capability', 'topicName', 'description'],
        data: []
      },
      filters: {},
      password: "",
      capabilityData: []
    },
    methods: {
      getCapabilityById: function(capabilityId) {
        return this.capabilityData.find(cap => capabilityId.valueOf() == cap.id.valueOf());
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
          var topics = topicService.getAll();

          return jq.when(topics);
        })
        .then(topics => {
          var payload = [];

          topics.forEach(t => {
            var cap = this.getCapabilityById(t.capabilityId);
            var tableTopic = {
              capability: {name: cap.name, id: cap.id},
              description: t.description,
              topicName: t.name
            };

            payload.push(tableTopic);
          });

          return payload;
        })
        .then(filteredTopics => {
          // Filter so it only shows public topics
          this.tablesData.data = filteredTopics.filter(t => {
            var firstFourChars = t.topicName.substring(0, 4);
            return firstFourChars.valueOf() === "pub.";
          });
        });
    }
})

 