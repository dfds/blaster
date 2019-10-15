import Vue from "vue";

const BannerComponent = Vue.component("banner", {
    props: ["title", "description"],
    data: function () {
        return {

        }
    },
    methods: {

    },
    template: `
        <div class="banner" style="display: flex; flex-direction: row; align-items: center; justify-content: center;">
            <p>{{ description }}</p>
        </div>
    `
});

export default BannerComponent;
export {BannerComponent};