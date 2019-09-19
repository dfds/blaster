import Vue from "vue";

const ChannelInputComponent = Vue.component("channel-input", {
    props: [],
    data: function() {
        return {
            inputText: "",
        }
    },
    methods: {
        onInput: function() {
            this.$emit('input-updated-query', this.inputText);
        }
    },
    template: `
        <div class="channelInput">
            <input type="text" v-model="inputText" @input="onInput" @focus="$emit('input-focus')" @blur="$emit('input-blur')">
        </div>
    `
});

export default ChannelInputComponent;
export {ChannelInputComponent};