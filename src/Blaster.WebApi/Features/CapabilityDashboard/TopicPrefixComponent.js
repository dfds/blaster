import Vue from "vue";

const TopicPrefixComponent = Vue.component("topic-prefix", {
    props: ["enable", "initialdata"],
    data: function() {
        return {
            businessArea: "",
            commonPrefix: "",
            type: ""
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

        toSnakeCase: function(input) {
            return input.replace(/\W+/g, " ")
            .split(/ |\B(?=[A-Z])/)
            .map(word => word.toLowerCase())
            .join('_');
        }
    },
    updated: function() {
        if (!this.enable) {
            //this.commonPrefix = "";
        }
    },
    mounted: function() {
        console.log(this.initialdata);
        this.commonPrefix = this.initialdata;
    },
    watch: {
        initialdata: function(newData, oldData) {
            console.log(newData);
            if (newData) {
                this.commonPrefix = newData;
            }
        }
    },
    template: `
        <div class="modal" v-bind:class="{'is-active': this.isEnabledStyling}">
            <div class="modal-background" v-on:click="$emit('topicprefix-close')"></div>
            <div class="modal-content" style="width: 913px;">
                <div class="modal-card" style="width: 913px;">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Set Topic prefix</p>
                        <button class="delete" aria-label="close" data-behavior="close" v-on:click="$emit('topicprefix-close')"></button>
                    </header>
                    <div class="modal-card-body">
                        <div class="dialog-container">
                        <p class>Before a Topic can be created, a common prefix for this Capability must be set. Said prefix will in combination with other prefixes make for more coherent naming, making it easier for other teams to get an overview.</p>                        
                        </div>
                        <div class="form">
                            <div class="field">
                                <label class="label">Common prefix</label>
                                <div>

                                </div>
                                <div class="control">
                                    <input class="input" type="text" data-property="commonPrefix" v-model="commonPrefix" :maxlength="32">
                                </div>
                                <p>Limitations: A max of 32 characters. The input will be transformed to snake case.</p>
                            </div>

                            <div>
                                <h3 style="font-weight: 700;">Examples</h3>
                                <p>Listed below is a few examples of how a final Topic name could end up looking with the common prefix set above.</p>
                                <div style="display: flex; flex-direction: column; margin-left: 20px; margin-top: 10px; margin-bottom: 10px;">
                                    <p style="font-size: 1.2rem;">
                                        build.{{ toSnakeCase(commonPrefix) }}.events.pelle
                                        <br />
                                        logistics.{{ toSnakeCase(commonPrefix) }}.logging
                                        <br />
                                        shipping.{{ toSnakeCase(commonPrefix) }}.events.pelle.subpelle
                                        <br />
                                        pax.{{ toSnakeCase(commonPrefix) }}.pelle
                                    </p>
                                </div>
                                <p>Do note that they examples shown above are just that, examples. The only thing you will be setting at this moment in time is the "common prefix".</p>
                                <p style="margin-top: 15px;">As you may have noticed, there is similar wording in some of the prefixes. According to the set naming convention, there is a few requirements as well as suggestions for naming a Topic. They are as following:
                                   <br />
                                    <ol style="margin-left: 40px;">
                                        <li>All topics start with a prefix denoting "business area": logistics, pax, shipping, build, other</li>
                                        <li>Self-chosen name, defaulted to the capability name. Whatever is chosen must be transformed to snake case, max of 32 characters</li>
                                        <li>Next segment can be a 'type': events, streaming, tracking, logging or blank</li>
                                        <li>Fourth or more is free</li>
                                    </ol>
                                </p>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('topicprefix-close', businessArea, self, type)">Save</button>
                                    <button class="button is-info" aria-label="close" data-behavior="close" v-on:click="$emit('topicprefix-close')">Cancel</button>
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

export default TopicPrefixComponent;
export {TopicPrefixComponent};