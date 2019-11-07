import Vue from "vue";

const TopicEditComponent = Vue.component("topic-edit", {
    props: ["enable", "initialdata", "capabilityname"],
    data: function() {
        return {
            topicId: "",
            topicDescription: "",
            topicMisc: ""
        }
    },
    computed: {
        isEnabledStyling: function() {
            return this.enable;
        },
        isMiscInUse: function() {
            return this.topicMisc !== "";
        },
        topicName: function() {
            var name = "";
            name = this.toSnakeCase(this.capabilityname);

            if (this.isMiscInUse) {
                name = name + "." + this.toSnakeCase(this.topicMisc);
            }            

            return name;
        }
    },
    methods: {
        disable: function() {
            this.enable = false;
        },
        toSnakeCase: function(input) {
            return input.replace(/\W+/g, " ")
            .split(/ |\B(?=[A-Z])/)
            .map(word => word.toLowerCase())
            .join('_');
        }
    },
    updated: function() {
        if (!this.enable) {
            this.topicDescription = "";
            this.topicMisc = "";
        }
    },
    watch: {
        initialdata: function(newData, oldData) {
            if (newData) {
                console.log(newData);
                this.topicDescription = newData.description;
                this.topicId = newData.id;
                this.topicMisc = newData.nameMisc;
            }
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('edittopic-close')"></div>
            <div class="modal-content" style="width: 80%; max-width: 650px;">
                <div class="modal-card" style="width: 100%;">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('edittopic-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <label class="label">Name</label>
                                <div style="display:flex; align-items: flex-end;">
                                    {{toSnakeCase(capabilityname)}}<span style="font-weight: 700;">.</span>
                                    <div style="display: flex; flex-direction: column; align-items: center;">
                                    <span style="font-weight: 700;">Name</span>
                                        <input style="width: 180px;" class="input" type="text" data-property="free" v-model="topicMisc">
                                    </div>
                                </div>
                                <div style="display:flex; flex-direction: column; justify-content: center; align-items: center; margin-top: 20px; margin-bottom: 10px;">
                                    <h3 style="font-size: 1.2rem; font-weight: 700;">Preview of name</h3>
                                    <br />
                                    <div style="display: flex; flex-direction: row; font-size: 1.2rem;">{{ topicName }}</div>
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">Description</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Description" data-property="description" v-model="topicDescription">
                                </div>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('edittopic-edit', topicName, topicDescription, topicId, topicMisc)">Save</button>
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