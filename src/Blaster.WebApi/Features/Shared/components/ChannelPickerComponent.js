import Vue from "vue";

import ChannelInputComponent from "./ChannelInputComponent";
import ChannelDropdownComponent from "./ChannelDropdownComponent";

const ChannelPickerComponent = Vue.component("channel-picker", {
    props: [],
    components: {
        'channel-input': ChannelInputComponent,
        'channel-dropdown': ChannelDropdownComponent,
    },
    data: function() {
        return {
            inputText: "",
            slackChannels: [],
            selectedSlackChannels: [],
            dropdownVisibility: false
        }
    },
    methods: {
        getChannels: function() {
            return [{name: "ded-infrastructure", id: "1"}, {name: "dev-excellence", id: "2"}, {name: "kubernetes", id: "3"}];
        },
        inputUpdatedQuery: function(query) {
            this.inputText = query;
        }
    },
    computed: {
        dropdownChannels: function() {
            if (this.inputText === "") {
                return this.slackChannels;
            } else {
                return this.slackChannels.filter(ch => ch.name.toLowerCase().includes(this.inputText.toLowerCase()));
            }
        }
    },
    beforeMount: function() {
        // GET list of usable Slack channels
        this.slackChannels = this.getChannels();
    },
    template: `
        <div class="channelPicker">
            <channel-input
                v-on:input-updated-query="inputUpdatedQuery"
                v-on:input-focus="dropdownVisibility = true"
                v-on:input-blur="dropdownVisibility = false">
            </channel-input>
            <channel-dropdown v-if="dropdownVisibility" :channels="dropdownChannels"></channel-dropdown>
        </div>
    `
});

export default ChannelPickerComponent;
export {ChannelPickerComponent};