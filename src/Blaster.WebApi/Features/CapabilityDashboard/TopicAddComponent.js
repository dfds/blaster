import Vue from "vue";

const TopicAddComponent = Vue.component("topic-add", {
    props: ["enable"],
    data: function() {
        return {
            topicName: "",
            topicDescription: "",
            topicPublic: true
        }
    },
    computed: {
        isEnabledStyling: function() {
            return this.enable;
        }
    },
    methods: {
        disable: function() {
            this.enable = false;
        }
    },
    updated: function() {
        if (!this.enable) {
            this.topicName = "";
            this.topicDescription = "";
            this.topicPublic = true;
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('addtopic-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Add Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('addtopic-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <label class="label">Name</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Enter capability name" data-property="name" v-model="topicName">
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">Description</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Description" data-property="description" v-model="topicDescription">
                                </div>
                            </div>
                            <div class="field" style="display: inline-flex;">
                                <label class="label">Public</label>
                                <div class="control" style="margin-left: 6px;">
                                    <input class="checkbox" type="checkbox" placeholder="Public" data-property="Public" v-model="topicPublic">
                                </div>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('addtopic-new-topic', topicName, topicDescription, topicPublic)">Save</button>
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