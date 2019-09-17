import Vue from "vue";

const ChannelInputComponent = Vue.component("channel-input", {
    props: [],
    data: function() {
        return {
            inputText: "",
            slackChannels: []
        }
    },
    methods: {
        onInput: function() {
            this.$emit('input-updated-query', this.inputText);
        },
        getChannels: function() {
            return [{name: "ded-infrastructure", id: "1"}, {name: "dev-excellence", id: "2"}, {name: "kubernetes", id: "3"}];
        }
    },
    computed: {
        dropdownChannels: function() {
            return this.slackChannels.filter(ch => ch.name.toLowerCase().includes(this.inputText.toLowerCase()));
        }
    },
    beforeMount: function() {
        // GET list of usable Slack channels
        this.slackChannels = this.getChannels();
    },
    template: `
        <div class="channelInput">
            <input type="text" v-model="inputText" @input="onInput" @focus="$emit('input-focus')" @blur="$emit('input-blur')">
        </div>
    `
});

export default ChannelInputComponent;
export {ChannelInputComponent};