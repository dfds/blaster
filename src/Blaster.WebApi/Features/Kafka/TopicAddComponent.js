import Vue from "vue";
import TopicService from "./topicservice";
import GenericWarningBox from "../Shared/components/GenericWarningBox";

const TopicAddComponent = Vue.component("topic-add", {
	props: ["enable", "capabilityId"],
	mounted: function () {
	},
	data: function () {
		return {
			topicDescription: "",
			topicNameInput: "",
			topicPartitions: 12,
			topicRetentionPeriodInDays: 7,
			topicNamePreview: "",
			topicName: "",
			topicService: new TopicService(),
			err: null
		}
	},
	components: {
		'generic-warning-box': GenericWarningBox
	},
	watch: {
		topicNameInput(value) {
      var configurations = {
        "retention.ms": parseInt(this.topicRetentionPeriodInDays, 10)
      };

			this.topicService
				.add(
					this.capabilityId,
					{
						"name": value,
						"partitions": parseInt(this.topicPartitions, 10),
						"retentionPeriodInDays": parseInt(this.topicRetentionPeriodInDays, 10),
						"description": this.topicDescription,
            "dryrun": true,
            "configurations": configurations
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
			this.topicService
				.add(
					this.capabilityId,
					{
						"name": this.topicNameInput,
						"partitions": parseInt(this.topicPartitions, 10),
						"retentionPeriodInDays": parseInt(this.topicRetentionPeriodInDays, 10),
						"description": this.topicDescription,
						"dryrun": false
					}
				)
				.then(() => {
						this.$emit('topicAdded');
						this.topicNameInput = "";
						this.topicPartitions = 12;
						this.topicRetentionPeriodInDays = 7;
						this.topicDescription = "";
					}
				);
		},
		disable: function () {
			this.enable = false;
		},
		toSnakeCase: function (input) {
			return input.replace(/\W+/g, " ")
				.split(/ |\B(?=[A-Z])/)
				.map(word => word.toLowerCase())
				.join('_');
		}
	},
	updated: function () {
		if (!this.enable) {
			this.topicDescription = "";
			this.topicMisc = "";
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
									<label class="label">partitions</label>
									<div class="control">
									<input type="radio" id="one" value="1" v-model="topicPartitions" :checked="checked">
									<label for="one">1</label>
									<input type="radio" id="one" value="3" v-model="topicPartitions" :checked="checked">
									<label for="three">3</label>
									<input type="radio" id="one" value="6" v-model="topicPartitions" :checked="checked">
									<label for="six">6</label>
									<input type="radio" id="one" value="12" v-model="topicPartitions" :checked="checked">
									<label for="twelve">12</label>
                                </div>
							</div>
							<div class="field">
								<p>
									Our recommendation for a no frills productions ready topic is 12 partitions.<br />
									You are welcome to contact the development excellence department if you need a different partitions count than available in this ui. 
								</p>
							</div>
							<div class="field">
								<label class="label">retention period in days</label>
								<div class="control">
									<input type="radio" id="one" value="604800000" v-model="topicRetentionPeriodInDays">
									<label for="seven">7</label>
									<input type="radio" id="one" value="2678400000" v-model="topicRetentionPeriodInDays">
									<label for="thirtyone">31</label>
									<input type="radio" id="one" value="31536000000" v-model="topicRetentionPeriodInDays">
									<label for="threehundredandsixtyfive">365</label>
									<input type="radio" id="one" value="-1" v-model="topicRetentionPeriodInDays">
									<label for="infinite">Infinity</label>
								</div>
							</div>
                            <div class="field">
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