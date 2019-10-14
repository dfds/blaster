import Vue from "vue";

const CapabilityRetireComponent = Vue.component("capability-retire", {
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
            <div class="modal-background" v-on:click="$emit('capability-retire-close')"></div>
            <div class="modal-content">
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Retire Capability</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('capability-retire-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container"></div>
                        <div class="form">
                            <div class="field">
                                <h2 class="label">Are you quite certain you wish to retire <span style="text-decoration: underline; color: #be1e2d">{{ capability.name }}</span>?</h2>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('capability-retire-save')">Retire</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('capability-retire-close')">Cancel</button>
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

export default CapabilityRetireComponent;
export {CapabilityRetireComponent};