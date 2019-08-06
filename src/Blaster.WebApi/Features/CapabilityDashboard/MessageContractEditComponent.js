import Vue from "vue";

const MessageContractEditComponent = Vue.component("message-contract-edit", {
    props: ["enable", "initialdata"],
    data: function() {
        return {
            mcDescription: "",
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
            this.mcDescription = "",
            this.mcType = "";
            this.mcSchema = "";
        }
    },
    watch: {
        initialdata: function(newData, oldData) {
            if (newData) {
                this.mcDescription = newData.description;
                this.mcType = newData.type;
                this.mcSchema = newData.content;
            }
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('messagecontractedit-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Add Message contract to Topic</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('messagecontractedit-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                        <div class="field">
                                <label class="label">Type</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Type" data-property="type" v-model="mcType" readonly>
                                </div>
                            </div>                            
                            <div class="field">
                                <label class="label">Description</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Description" data-property="description" v-model="mcDescription">
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
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('messagecontractedit-edit', mcType, mcDescription, mcSchema)">Save</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('messagecontractedit-close')">Cancel</button>
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

export default MessageContractEditComponent;
export {MessageContractEditComponent};