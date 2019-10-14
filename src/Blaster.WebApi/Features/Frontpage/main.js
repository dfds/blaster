import Vue from "vue";
import { currentUser } from "userservice";
import FeatureFlag from "featureflag";
import "core-js/stable";
import "regenerator-runtime/runtime";
import {isIE} from "../Shared/components/Shared";

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
        },
    },
    methods: {
    },
    filters: {
    },
    mounted: function () {
        this.initializing = false;
    }
});