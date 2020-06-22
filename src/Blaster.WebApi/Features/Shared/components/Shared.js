import Vue from "vue";
import UserManagementPlugin from "../plugins/userManagementPlugin";
import HttpClientPlugin from "../plugins/httpClientPlugin";
import ChannelPickerComponent from "./ChannelPickerComponent";
import ChannelMinimalComponent from "./ChannelMinimalComponent";
import ChannelListComponent from "./ChannelListComponent";
import ChannelDropdownComponent from "./ChannelDropdownComponent";
import ChannelIconComponent from "./ChannelIconComponent";
import ChannelInputComponent from "./ChannelInputComponent";
import BannerComponent from "./BannerComponent";
import {InstallRequestMsalHandler} from "../plugins/HttpHandlers/RequestMsalHandler";
import FeatureFlag from "featureflag";

Vue.prototype.$featureFlag = new FeatureFlag();

Vue.use(UserManagementPlugin);
Vue.use(HttpClientPlugin);
InstallRequestMsalHandler(Vue);

new Vue({
    el: ".navbar",
    computed: {
        showIEBanner: function () {
            return isIE();
        }
    },
    methods: {
        toggleActive: function () {
            this.active = !this.active;
        },
        generateCountdownTime: function(year, month, date) {
            var now = new Date();
            var newYear = new Date(year, month, date);
            return newYear - now;
        }
    },
    computed: {
        isActive: function() {
            return this.active;
        },
        time: function() {
            return this.generateCountdownTime("2020", "03", "30");
        }
    },
    data: {
        active: false,
        showPublicTopics: false
    },
    components: {
        'banner': BannerComponent
    },
    mounted: function() {
      this.showPublicTopics = this.$featureFlag.flagExists("publictopicspage") ? this.$featureFlag.getFlag("publictopicspage").enabled : false;
    }
    
});



function isIE()
{
    const ua = window.navigator.userAgent;

    const msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    const trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }
}

export {BannerComponent, ChannelPickerComponent, ChannelListComponent, ChannelMinimalComponent, ChannelDropdownComponent, ChannelIconComponent, ChannelInputComponent, isIE};