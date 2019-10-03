import Vue from "vue";

import ChannelIconComponent from "./ChannelIconComponent";

const ChannelMinimalComponent = Vue.component("channel-minimal", {
    props: ["channel", "enablehover"],
    components: {
        'channel-icon': ChannelIconComponent
    },
    computed: {
        useHighlight: function () {
            if (this.enablehover) {
                return this.enablehover;
            } else {
                return false;
            }
        }
    },
    template: `
        <div class="channel" v-bind:class="{ channelhightlight: useHighlight }">
            <channel-icon :type="channel.type"></channel-icon> #{{ channel.name }}
        </div>
    `
});

export default ChannelMinimalComponent;
export {ChannelMinimalComponent};