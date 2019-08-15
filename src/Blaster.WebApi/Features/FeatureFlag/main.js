import Vue from "vue";
import FeatureFlag from "featureflag";
import Cookies from "js-cookie";

Vue.prototype.$featureFlag = new FeatureFlag();
const app = new Vue({
    el: "#capabilities-app",
    data: {
        initializing: true,
        flags: null,
    },
    computed: {
        hasFlags() {
            return (this.flags !== null);
        }
    },   
    methods: {
        init() {
            this.flags = this.$featureFlag.flags;

            return new Promise((resolve, reject) => {
                resolve('Ok');
            });
        }
    },
    filters: {

    },
    watch: {
        flags: {
            handler(val) {
                Cookies.set('blaster.ff', btoa(JSON.stringify(val)));
            },
            deep: true
        }
    },
    mounted: function () {
        this.init()
            .then(() => {
                this.initializing = false;
            });
    }
});