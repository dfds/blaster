import Vue from "vue";

import ChannelInputComponent from "./ChannelInputComponent";
import ChannelDropdownComponent from "./ChannelDropdownComponent";
import ChannelService from "channelservice";

const channelService = new ChannelService();

const ChannelPickerComponent = Vue.component("channel-picker", {
    props: [],
    components: {
        'channel-input': ChannelInputComponent,
        'channel-dropdown': ChannelDropdownComponent,
    },
    data: function() {
        return {
            inputText: "",
            channels: [],
            selectedChannels: [],
            dropdownVisibility: false
        }
    },
    methods: {
        getChannels: function() {
            channelService.getAll()
                .then((items) => {
                    this.channels = items;
                });
        },
        inputUpdatedQuery: function(query) {
            this.inputText = query;
        }
    },
    computed: {
        dropdownChannels: function() {
            if (this.inputText === "") {
                return this.channels;
            } else {
                return this.channels.filter(ch => ch.name.toLowerCase().includes(this.inputText.toLowerCase()));
            }
        }
    },
    beforeMount: function() {
        this.getChannels();
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