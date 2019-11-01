import Vue from "vue";
import FeatureFlag from "featureflag";
import "core-js/stable";
import "regenerator-runtime/runtime";
import { isIE, BannerComponent } from "../Shared/components/Shared";

FeatureFlag.setKeybinding();

Vue.prototype.$featureFlag = new FeatureFlag();

const app = new Vue({
    el: "#frontpage-app",
    data: {
        initializing: true,
        currentUser: currentUser
    },
    computed: {
        showIEBanner: function() {
            return isIE();
        }
    },
    components: {
        'banner': BannerComponent
    },
    methods: {
    },
    filters: {
    },
    mounted: function () {
        this.initializing = false;
    }
});