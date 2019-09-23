import Vue from "vue";

import ChannelMinimalComponent from "./ChannelMinimalComponent";

const ChannelListComponent = Vue.component("channel-list", {
    props: ["channels"],
    components: {
        'channel-minimal': ChannelMinimalComponent,
    },
    data: function () {
        return {

        }
    },
    methods: {
        leaveChannel: function(channel) {
            this.$emit('capability-leave-channel', channel);
        }
    },
    template: `
        <div class="channelList" style="display: flex; flex-direction: column;">
            <div style="display: flex; flex-direction: row; align-items: center;" v-for="channel in channels" :key="channel.id">
                <a class="delete" style="margin-right: 5px;" @click="leaveChannel(channel)"></a>            
                <channel-minimal :channel="channel" :enablehover=false></channel-minimal> 
            </div>
        </div>
    `
});

export default ChannelListComponent;
export {ChannelListComponent};