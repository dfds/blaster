import Vue from "vue";

const ChannelComponent = Vue.component("channel-view", {
    props: ["channel"],
    template: `
        <div class="channel">
            #{{ channel.name }}
        </div>
    `
});

const ChannelDropdownComponent = Vue.component("channel-dropdown", {
    props: ["channels"],
    components: {
        'channel-view': ChannelComponent
    },
    data: function() {
        return {

        }
    },
    template: `
        <div class="channelDropdown">
            <channel-view v-for="channel in channels" :key="channel.id" :channel="channel"></channel-view>
        </div>
    `
});

export default ChannelDropdownComponent;
export {ChannelDropdownComponent};