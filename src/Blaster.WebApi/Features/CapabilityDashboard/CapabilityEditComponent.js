import Vue from "vue";

const CapabilityEditComponent = Vue.component("capability-edit", {
    props: ["enable", "initialdata"],
    data: function() {
        return {
            description: this.initialdata.description,
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
        },
        generateUpdatedCapability: function() {
            return {
                description: this.description,
                name: this.initialdata.name
            }
        }
    },
    watch: {
        initialdata: function(newData, oldData) {
            if (newData) {
                this.description = newData.description;
            }
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('capability-edit-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Capability</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('capability-edit-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <label class="label">Description</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Short and explanatory piece on what this Capability is set out to do" data-property="description" v-model="description">
                                </div>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('capability-edit-save', generateUpdatedCapability())">Save</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('capability-edit-close')">Cancel</button>
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

export default CapabilityEditComponent;
export {CapabilityEditComponent};