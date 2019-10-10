import Vue from "vue";

import ChannelInputComponent from "./ChannelInputComponent";
import ChannelDropdownComponent from "./ChannelDropdownComponent";
import ChannelService from "channelservice";

const channelService = new ChannelService();

const ChannelPickerComponent = Vue.component("channel-picker", {
    props: ["capabilitychannels", "is_enabled"],
    components: {
        'channel-input': ChannelInputComponent,
        'channel-dropdown': ChannelDropdownComponent,
    },
    data: function() {
        return {
            inputText: "",
            channels: [],
            selectedChannels: [],
            inputFocus: false
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
        },
        onInputBlur: function() {
            // Ain't pretty, but due to the nature of
            setTimeout(() => {
                this.inputFocus = false;
            }, 300);
        },
        onChannelClick: function(channel) {
            this.$emit('capability-join-channel', channel);
        },
        filterCapabilityChannels: function() {
            if (this.capabilitychannels) {
                return this.channels.filter(ch => {
                    for (var i = 0; i < this.capabilitychannels.length; i++) {
                        if ((ch.id.valueOf() === this.capabilitychannels[i].id.valueOf())) {
                            return false;
                        }
                    }
                    return true;
                });
            } else {
                return this.channels;
            }
        }
    },
    computed: {
        dropdownChannels: function() {
            if (this.inputText === "") {
                return this.filterCapabilityChannels();
            } else {
                return this.filterCapabilityChannels().filter(ch => ch.name.toLowerCase().includes(this.inputText.toLowerCase()));
            }
        },
        showDropdown: function() {
            if (this.inputFocus) {
                return "flex";
            } else {
                return "none";
            }
        }
    },
    beforeMount: function() {
        this.getChannels();
    },
    template: `
        <div class="channelPicker">
            <channel-input
                :is_enabled="is_enabled" 
                v-on:input-updated-query="inputUpdatedQuery"
                v-on:input-focus="inputFocus = true"
                v-on:input-blur="onInputBlur">
            </channel-input>
            <channel-dropdown
                v-bind:style="{ display: showDropdown }"
                v-on:capability-join-channel="onChannelClick"
                v-if="is_enabled"
                :channels="dropdownChannels">
            </channel-dropdown>
        </div>
    `
});

export default ChannelPickerComponent;
export {ChannelPickerComponent};
