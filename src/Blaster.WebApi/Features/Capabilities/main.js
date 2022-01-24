import Vue from "vue";
import CapabilityService from "capabilityservice"
import AlertDialog from "./alert-dialog";
import ModelEditor from "modeleditor";
import jq from "jquery";
import { UserService } from "userservice";
import FeatureFlag from "featureflag";
import TestCapabilitiesFiltered from './test-filter';
import "core-js/stable";
import "regenerator-runtime/runtime";
import {isIE, isFF, BannerComponent} from "../Shared/components/Shared";

const capabilityService = new CapabilityService(Vue.prototype.$http, Vue.prototype.$userService);
FeatureFlag.setKeybinding();

Vue.prototype.$featureFlag = new FeatureFlag();

const app = new Vue({
    el: "#capabilities-app",
    data: {
        items: [],
        membershipRequests: [],
        initializing: true,
        currentUser: Vue.prototype.$userService.getCurrentUser()
    },
    computed: {
        hasCapabilities: function () {
            return this.items.length > 0;
        },
        showIEBanner: function() {
            return isIE();
        },
        showFFBanner: function() {
            return isFF();
        }
    },
    components: {
        'banner': BannerComponent
    },
    methods: {
        newCapability: function() {
            const editor = ModelEditor.open({
                template: document.getElementById("editor-template"),
                data: {
                    name: "",
                    description: "",
                },
                onClose: () => editor.close(),
                onSave: (capabilityData) => {
                    return capabilityService.add(capabilityData)
                        .then(capability => this.items.push(capability.data))
                        .then(() => editor.close())
                        .catch(err => {
                            const dialog = AlertDialog.open({
                                template: document.getElementById("error-dialog-template"),
                                container: jq(".dialog-container", editor.element),
                                data: {
                                    title: "Error",
                                    message: err.response.data.message
                                }
                            });

                            setTimeout(function() {
                                dialog.close();
                            }, 15000);

                        });
                }
            });
        },
        getMembershipStatusFor: function(capabilityId) {
            const capability = this.items.find(capability => capability.id == capabilityId);
            const isRequested = this.membershipRequests.indexOf(capability.id) > -1;

            if (isRequested) {
                return "requested";
            }

            return this._isCurrentlyMemberOf(capability)
                ? "member"
                : "notmember";
        },
        _isCurrentlyMemberOf: function(capability) {
            if (!capability) {
                return false;
            }

            const members = capability.members || [];
            return members
                .filter(member => member.email.toLowerCase() == this.getUserEmail().toLowerCase())
                .length > 0;
        }
    },
    filters: {
        capabilitydetails: function(capabilityId) {
            return `/capabilitydashboard?capabilityId=${capabilityId}`
        }
    },
    mounted: function () {
        jq.ready
            .then(() => {
                var x = capabilityService.getAll();

                return jq.when(x);
            })
            .then(resp => resp.items)
            .then(capabilities => capabilities.forEach(capability => this.items.push(capability)))
            .then(() => {
                if (!this.$featureFlag.getFlag("testfilter").enabled) {
                    this.items = this.items.filter(item => !TestCapabilitiesFiltered.has(item.id));
                }
            })
            .catch(info => {
                if (info.status != 200) {
                    AlertDialog.open({
                        template: document.getElementById("error-dialog-template"),
                        container: document.getElementById("global-dialog-container"),
                        data: {
                            title: "Error!",
                            message: `Could not retrieve list of capabilities. Server returned (${info.status}) ${info.statusText}.`
                        }
                    });
                }
            })
            .done(() => this.initializing = false);
    }
});