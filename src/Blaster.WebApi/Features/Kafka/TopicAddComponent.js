import Vue from "vue";
import TopicService from "./topicservice";
import GenericWarningBox from "../Shared/components/GenericWarningBox";

const TopicAddComponent = Vue.component("topic-add", {
	props: ["enable", "capabilityId", "clusters"],
	mounted: function () {
	},
	data: function () {
		return this.getInitData();
  },
  updated: function () {
		if (!this.enable) {
      		var initData = this.getInitData();
			this.topicDescription = initData.topicDescription;
      		this.topicMisc = initData.topicMisc;
			this.topicNameInput = initData.topicNameInput;
			this.topicPartitions = initData.topicPartitions;
			this.topicRetentionPeriodInMs = initData.topicRetentionPeriodInMs;
			this.topicNamePreview = initData.topicNamePreview;
      		this.topicName = initData.topicName;
			this.err = initData.err;
			this.topicAvailability = initData.topicAvailability;
      this.topicCluster = initData.topicCluster;
		}
	},
	components: {
		'generic-warning-box': GenericWarningBox
	},
	watch: {
		topicNameInput(value) {
      var configurations = {
        "retention.ms": parseInt(this.topicRetentionPeriodInMs, 10)
      };

			this.topicService
				.add(
					this.capabilityId,
					{
						"name": value,
						"partitions": parseInt(this.topicPartitions, 10),
						"retentionPeriodInDays": parseInt(this.topicRetentionPeriodInMs, 10),
						"description": this.topicDescription,
						"dryrun": true,
						"configurations": configurations,
						"availability" : this.topicAvailability,
            "kafkaClusterId": this.topicCluster
					}
				)
				.then(r => {
					this.topicNamePreview = r.name;
					this.err = null;
				})
				.catch(err => {
					this.err = err;
					this.topicNamePreview = "";
				});
    },

    topicDescription(value) {
      if (this.topicNameInput.valueOf() !== "".valueOf()) {
        var configurations = {
          "retention.ms": parseInt(this.topicRetentionPeriodInMs, 10)
        };
  
        this.topicService
          .add(
            this.capabilityId,
            {
              "name": this.topicNameInput,
              "partitions": parseInt(this.topicPartitions, 10),
              "retentionPeriodInDays": parseInt(this.topicRetentionPeriodInMs, 10),
              "description": this.topicDescription,
              "dryrun": true,
              "configurations": configurations,
              "availability" : this.topicAvailability,
              "kafkaClusterId": this.topicCluster
            }
          )
          .then(r => {
            this.topicNamePreview = r.name;
            this.err = null;
          })
          .catch(err => {
            this.err = err;
            this.topicNamePreview = "";
          });
      }
    },
    
    topicAvailability(value) {
      if (this.topicNameInput.valueOf() !== "".valueOf()) {
        var configurations = {
          "retention.ms": parseInt(this.topicRetentionPeriodInMs, 10)
        };
  
        this.topicService
          .add(
            this.capabilityId,
            {
              "name": this.topicNameInput,
              "partitions": parseInt(this.topicPartitions, 10),
              "retentionPeriodInDays": parseInt(this.topicRetentionPeriodInMs, 10),
              "description": this.topicDescription,
              "dryrun": true,
              "configurations": configurations,
              "availability" : this.topicAvailability,
              "kafkaClusterId": this.topicCluster
            }
          )
          .then(r => {
            this.topicNamePreview = r.name;
            this.err = null;
          })
          .catch(err => {
            this.err = err;
            this.topicNamePreview = "";
          });
      }
    },

    topicCluster(value) {
      if (this.topicNameInput.valueOf() !== "".valueOf()) {
        var configurations = {
          "retention.ms": parseInt(this.topicRetentionPeriodInMs, 10)
        };
  
        this.topicService
          .add(
            this.capabilityId,
            {
              "name": this.topicNameInput,
              "partitions": parseInt(this.topicPartitions, 10),
              "retentionPeriodInDays": parseInt(this.topicRetentionPeriodInMs, 10),
              "description": this.topicDescription,
              "dryrun": true,
              "configurations": configurations,
              "availability" : this.topicAvailability,
              "kafkaClusterId": this.topicCluster
            }
          )
          .then(r => {
            this.topicNamePreview = r.name;
            this.err = null;
          })
          .catch(err => {
            this.err = err;
            this.topicNamePreview = "";
          });
      }
    }    

	},
	computed: {
		isEnabledStyling: function () {
			return this.enable;
		},
		isMiscInUse: function () {
			return this.topicMisc !== "";
		},
	},
	methods: {
		saveTopic: function () {
      var configurations = {
        "retention.ms": parseInt(this.topicRetentionPeriodInMs, 10)
      };

			this.topicService
				.add(
					this.capabilityId,
					{
						"name": this.topicNameInput,
						"partitions": parseInt(this.topicPartitions, 10),
						"retentionPeriodInDays": parseInt(this.topicRetentionPeriodInMs, 10),
						"description": this.topicDescription,
            "dryrun": false,
            "configurations": configurations,
						"availability" : this.topicAvailability,
            "kafkaClusterId": this.topicCluster
					}
				)
				.then(() => {
						this.$emit('topicAdded');
						this.topicNameInput = "";
						this.topicPartitions = 1;
						this.topicRetentionPeriodInMs = 604800000;
						this.topicDescription = "";
						this.topicAvailability = "private";
            this.topicCluster = "";
					}
        )
        .catch(err => {
					this.err = err;
        });
		},
		disable: function () {
			this.enable = false;
		},
		toSnakeCase: function (input) {
			return input.replace(/\W+/g, " ")
				.split(/ |\B(?=[A-Z])/)
				.map(word => word.toLowerCase())
				.join('_');
    },
    getInitData: function() {
      return {
        topicDescription: "",
        topicNameInput: "",
        topicPartitions: 1,
        topicRetentionPeriodInMs: 604800000,
        topicNamePreview: "",
        topicName: "",
        topicAvailability: "private",
        topicService: new TopicService(),
        topicCluster: "",
        err: null
      }
    }
	},
	template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('addtopic-close')"></div>
            <div class="modal-content" style="width: 80%; max-width: 650px;">
                <div class="modal-card" style="width: 100%;">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Add Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('addtopic-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <label class="label">Name</label>

                                <p style="margin-bottom: 5px;">
                                  The following characters are allowed in Topic names:
                                  <ul style="list-style-type:disc; margin-left: 30px;">
                                    <li>a-Z</li>
                                    <li>0-9</li>
                                    <li>-</li>
                                  </ul>
                                  <br />
                                  Everything else will be corrected on your behalf. Max characters including the automatically appended prefix is 55 characters.
                                </p>

								<input class="input" type="text" data-property="free" v-model="topicNameInput">
                                <div style="display:flex; flex-direction: column; justify-content: center; align-items: center; margin-top: 20px; margin-bottom: 10px;">
                                    <h3 style="font-size: 1.2rem; font-weight: 700;">Preview of name</h3>

                                    <br />
                                    <div style="display: flex; flex-direction: row; font-size: 1.2rem; word-break: break-all;">{{ topicNamePreview }}</div>
                                </div>
								<generic-warning-box :err="err"></generic-warning-box>
                            </div>
                            <div class="field">
                                <label class="label">Description</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Description" data-property="description" v-model="topicDescription">
                                </div>
							</div>
							<div class="field">
								<label class="label">Availability</label>
								<div class="control">
									<input type="radio" id="availabilityPrivate" value="private" v-model="topicAvailability" >
									<label>Private</label>
									<input type="radio" id="availabilityPublic" value="public" v-model="topicAvailability" >
									<label>Public</label>
								</div>
							</div>

							<div class="field">
								<p>
									Private topics can only be written to and read from the capability creating them.<br />
									Public topics can be written to from the capability creating them and read from all capabilities within DFDS.
								</p>
								<p>
									Availability can not be changed after creation of a topic. 
								</p>


                <div class="field">
								<label class="label">Cluster</label>
								<div class="select">
                  <select name="cluster" required v-model="topicCluster">
                    <option disabled selected value> -- select a cluster -- </option>
                    <option v-for="cluster in clusters" :key="cluster.id" :disabled="cluster.enabled ? false : true" :value="cluster.id">{{cluster.name}} ({{cluster.clusterId}})</option>
                  </select>
								</div>
							</div>


							</div>
						  	<div class="field">
								<label class="label">Partitions</label>
								<div class="control">
									<input type="radio" id="one" value="1" v-model="topicPartitions" >
									<label for="one">1</label>
									<input type="radio" id="one" value="3" v-model="topicPartitions" >
									<label for="three">3</label>
									<input type="radio" id="one" value="6" v-model="topicPartitions" >
									<label for="six">6</label>
                </div>
							</div>
							<div class="field">
                <p>
                  A single partition can support a very high load. You should normally only use more partitions if you need concurrent handling and/or lower latency.
								</p>
							</div>
							<div class="field">
								<label class="label">Retention period in days</label>
								<div class="control">
									<input type="radio" id="one" value="604800000" v-model="topicRetentionPeriodInMs">
									<label for="seven">7</label>
									<input type="radio" id="one" value="2678400000" v-model="topicRetentionPeriodInMs">
									<label for="thirtyone">31</label>
									<input type="radio" id="one" value="31536000000" v-model="topicRetentionPeriodInMs">
									<label for="threehundredandsixtyfive">365</label>
									<input type="radio" id="one" value="-1" v-model="topicRetentionPeriodInMs">
									<label for="infinite">Infinity</label>
								</div>
							</div>
                            <div class="field">
                                <p style="margin-bottom: 10px;">Provisioning of a Topic does typically take five minutes.</p>
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="saveTopic">Save</button>
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

export default TopicAddComponent;
export {TopicAddComponent};