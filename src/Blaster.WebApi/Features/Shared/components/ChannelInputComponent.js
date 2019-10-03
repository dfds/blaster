import Vue from "vue";

const ChannelInputComponent = Vue.component("channel-input", {
    props: ["is_enabled"],
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
            <input :disabled="!is_enabled" type="text" v-model="inputText" @input="onInput" @focus="$emit('input-focus')" @blur="$emit('input-blur')">
        </div>
    `
});

export default ChannelInputComponent;
export {ChannelInputComponent};