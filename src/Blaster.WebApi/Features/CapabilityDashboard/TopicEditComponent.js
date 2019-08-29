import Vue from "vue";

const TopicEditComponent = Vue.component("topic-edit", {
    props: ["enable", "initialdata", "commonprefix"],
    data: function() {
        return {
            topicDescription: "",
            topicPublic: true,
            topicId: "",
            topicBusinessArea: "",
            topicType: "",
            topicMisc: ""
        }
    },
    computed: {
        isEnabledStyling: function() {
            return this.enable;
        },
        isTypeInUse: function() {
            return this.topicType !== "";
        },
        isMiscInUse: function() {
            return this.topicMisc !== "";
        },
        topicName: function() {
            var name = "";
            name = this.topicBusinessArea + "." + this.commonprefix;
            if (this.isTypeInUse) {
                name = name + "." + this.topicType;
            }

            if (this.isMiscInUse) {
                name = name + "." + this.topicMisc;
            }            

            return name;
        }
    },
    methods: {
        disable: function() {
            this.enable = false;
        }
    },
    updated: function() {
        if (!this.enable) {
            this.topicDescription = "";
            this.topicPublic = true;
            this.topicId = "";
            this.topicBusinessArea = "";
            this.topicType = "";
            this.topicMisc = "";
        }
    },
    watch: {
        initialdata: function(newData, oldData) {
            if (newData) {
                console.log(newData);
                this.topicDescription = newData.description;
                this.topicPublic= newData.isPrivate;
                this.topicId = newData.id;
                this.topicBusinessArea = newData.nameBusinessArea;
                this.topicType = newData.nameType;
                this.topicMisc = newData.nameMisc;
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
                                <div style="display:flex; align-items: flex-end;">
                                    <div style="display: flex; flex-direction: column; align-items: center;">
                                        <span style="font-weight: 700;">Business area</span>
                                        <input style="width: 180px;" class="input" type="text" placeholder="Business area" data-property="businessArea" v-model="topicBusinessArea">
                                    </div>
                                    <span style="font-weight: 700;">.</span>{{commonprefix}}<span style="font-weight: 700;">.</span>
                                    <div style="display: flex; flex-direction: column; align-items: center;">
                                        <span style="font-weight: 700;">Type<span style="font-size: 0.8rem; font-weight: 300"> (optional)</span></span>
                                        <select style="width: 180px;" class="input" placeholder="Type" data-property="type" v-model="topicType">
                                            <option value=""></option>
                                            <option value="events">Events</option>
                                            <option value="streaming">Streaming</option>
                                            <option value="tracking">Tracking</option>
                                            <option value="logging">Logging</option>
                                        </select>
                                    </div><span style="font-weight: 700;">.</span>
                                    <div style="display: flex; flex-direction: column; align-items: center;">
                                    <span style="font-weight: 700;">Misc<span style="font-size: 0.8rem; font-weight: 300"> (optional)</span></span>
                                        <input style="width: 180px;" class="input" type="text" placeholder="Optional" data-property="free" v-model="topicMisc">
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
                            <div class="field" style="display: inline-flex;">
                                <label class="label">Public</label>
                                <div class="control" style="margin-left: 6px;">
                                    <input class="checkbox" type="checkbox" placeholder="Public" data-property="Public" v-model="topicPublic">
                                </div>
                            </div>
                            <div class="field">
                                <div class="control has-text-centered">
                                    <button class="button is-primary" data-behavior="save" v-on:click="$emit('edittopic-edit', topicName, topicDescription, topicPublic, topicId, topicBusinessArea, topicType, topicMisc)">Save</button>
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