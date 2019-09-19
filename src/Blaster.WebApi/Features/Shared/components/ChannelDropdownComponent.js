import Vue from "vue";

import ChannelMinimalComponent from "./ChannelMinimalComponent";

const ChannelDropdownComponent = Vue.component("channel-dropdown", {
    props: ["channels"],
    components: {
        'channel-minimal': ChannelMinimalComponent
    },
    data: function() {
        return {

        }
    },
    methods: {
        onChannelClick: function (channel) {
            this.$emit('capability-join-channel', channel);
        }
    },
    template: `
        <div class="channelDropdown" @focus="$emit('dropdown-focus')" @blur="$emit('dropdown-blur')">
            <channel-minimal v-for="channel in channels" :key="channel.id" :channel="channel" @click.native="onChannelClick(channel)"></channel-minimal>
        </div>
    `
});

export default ChannelDropdownComponent;
export {ChannelDropdownComponent};