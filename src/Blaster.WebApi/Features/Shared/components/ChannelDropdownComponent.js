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
            console.log(channel.name + " was clicked on");
        }
    },
    template: `
        <div class="channelDropdown">
            <channel-minimal v-for="channel in channels" :key="channel.id" :channel="channel" v-on:click.native="onChannelClick(channel)"></channel-minimal>
        </div>
    `
});

export default ChannelDropdownComponent;
export {ChannelDropdownComponent};