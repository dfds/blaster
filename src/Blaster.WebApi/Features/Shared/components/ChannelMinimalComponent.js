import Vue from "vue";

import ChannelIconComponent from "./ChannelIconComponent";

const ChannelMinimalComponent = Vue.component("channel-minimal", {
    props: ["channel"],
    components: {
        'channel-icon': ChannelIconComponent
    },
    template: `
        <div class="channel">
            <channel-icon :type="channel.type"></channel-icon> #{{ channel.name }}
        </div>
    `
});

export default ChannelMinimalComponent;
export {ChannelMinimalComponent};