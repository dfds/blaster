import Vue from "vue";
import "core-js/stable";
import "regenerator-runtime/runtime";

import {isIE, BannerComponent} from "../Shared/components/Shared";


const app = new Vue({
    el: "#login-app",
    components: {
        'banner': BannerComponent
    },
    computed: {
        showIEBanner: function() {
            return isIE();
        }
    },
})