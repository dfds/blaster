import Vue from "vue";

const MessageContractAddComponent = Vue.component("message-contract-add", {
    props: ["enable"],
    data: function() {
        return {
            mcTitle: "",
            mcType: "",
            mcSchema: ""
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
            this.mcTitle = "";
            this.mcType = "";
            this.mcSchema = "";
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('messagecontractadd-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Add Message contract to Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('messagecontractadd-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                        <div class="field">
                                <label class="label">Title</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Title" data-property="title" v-model="mcTitle">
                                </div>
                            </div>                            
                            <div class="field">
                                <label class="label">Type</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Select type" data-property="type" v-model="mcType">
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">Content</label>
                                <div class="control">
                                    <textarea class="textarea" placeholder="Schema" data-property="schema" v-model="mcSchema"></textarea>
                                </div>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('messagecontractadd-new', mcTitle, mcType, mcSchema)">Save</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('messagecontractadd-close')">Cancel</button>
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

export default MessageContractAddComponent;
export {MessageContractAddComponent};