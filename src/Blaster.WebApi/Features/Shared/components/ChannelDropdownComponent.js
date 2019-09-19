import Vue from "vue";

const ChannelIcon = Vue.component("channel-icon", {
    props: ["type"],
    data: function() {
        return {
            enable: false,
            src: ""
        }
    },
    template: `
        <img v-if="enable" :src="src" style="width: 25px; height: 25px; min-width: 25px; min-height: 25px; margin-right: 5px;" />
    `,
    beforeMount: function() {
        // TODO: Redo the entirety of this
        if (this.type) {
            if (this.type.valueOf() === new String("slack").valueOf()) {
                this.enable = true;
                this.src = "/img/slack.svg";
            }

            if (this.type.valueOf() === new String("msteams").valueOf()) {
                this.enable = true;
                this.src = "/img/msteams.svg";
            }
        } else {
            this.enable = false;
        }
    }
});

const ChannelComponent = Vue.component("channel-view", {
    props: ["channel"],
    components: {
        'channel-icon': ChannelIcon
    },
    template: `
        <div class="channel">
            <channel-icon :type="channel.type"></channel-icon> #{{ channel.name }}
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