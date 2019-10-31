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
        requireAuthOnInitialize: false,
        postLogoutRedirectUri: "http://localhost:5000"
    }
});

FeatureFlag.setKeybinding();

Vue.prototype.$featureFlag = new FeatureFlag();
const app = new Vue({
    el: "#app-container",
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
        getUserName: function () {
            let userName = currentUser.email;

            if (this.$msal.isAuthenticated()) {
                userName = this.$msal.data.user.name.split(' ');
                userName = userName[1] + ' ' + userName[0];
            }

            return userName;
        },
        signIn: function() {
            if (!this.$msal.isAuthenticated()) {
                this.$msal.signIn();
            }
        },
        isAuthenticated: function () {
            return this.$msal.isAuthenticated();
        },
        signOut: function () {
            if (this.$msal.isAuthenticated()) {
                this.$msal.signOut();
            }
        }
    },
    filters: {
    },
    mounted: function () {
        this.initializing = false;
    }
});