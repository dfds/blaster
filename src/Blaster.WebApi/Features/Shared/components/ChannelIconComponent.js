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

export default ChannelIcon;
export {ChannelIcon};