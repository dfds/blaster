import Vue from "vue";
import { currentUser } from "userservice";
import FeatureFlag from "featureflag";
import "core-js/stable";
import "regenerator-runtime/runtime";
import {isIE, BannerComponent} from "../Shared/components/Shared";
import msal from 'vue-msal'

Vue.use(msal, {
    auth: {
        clientId: '91c38c20-4d2c-485d-80ac-a053619a02db',
        requireAuthOnInitialize: true
    }
});

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