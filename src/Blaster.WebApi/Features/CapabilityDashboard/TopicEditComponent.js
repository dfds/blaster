import Vue from "vue";

const TopicEditComponent = Vue.component("topic-edit", {
    props: ["enable", "initialdata"],
    data: function() {
        return {
            topicName: "",
            topicDescription: "",
            topicPublic: true,
            topicId: ""
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
            this.topicId = "";
        }
    },
    watch: {
        initialdata: function(newData, oldData) {
            if (newData) {
                this.topicName = newData.name;
                this.topicDescription = newData.description;
                this.topicPublic= newData.isPrivate;
                this.topicId = newData.id;
            }
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('edittopic-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('edittopic-close')"></button>
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
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('edittopic-edit', topicName, topicDescription, topicPublic, topicId)">Save</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('edittopic-close')">Cancel</button>
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

export default TopicEditComponent;
export {TopicEditComponent};