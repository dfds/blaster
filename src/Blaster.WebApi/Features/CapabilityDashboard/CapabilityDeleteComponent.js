import Vue from "vue";

const CapabilityDeleteComponent = Vue.component("capability-delete", {
    props: ["enable", "capability"],
    data: function() {
        return {
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
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('capability-delete-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Delete Capability</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('capability-delete-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <h2 class="label">Are you quite certain you wish to delete <span style="text-decoration: underline; color: #be1e2d">{{ capability.name }}</span>?</h2>
                                <p>
                                The namespace(s) in Kubernetes coupled to this capability will be deleted.<br>
                                The slack channels connected to this capability will be archived. <br>
                                Please de-provision any resources in the AWS account coupled to this capability.
                                </p>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('capability-delete-save')">Delete</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('capability-delete-close')">Cancel</button>
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

export default CapabilityDeleteComponent;
export {CapabilityDeleteComponent};