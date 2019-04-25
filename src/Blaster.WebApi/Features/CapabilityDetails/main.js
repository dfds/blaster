import Vue from "vue";
import ModelEditor from "modeleditor";
import jq from "jquery";
import { currentUser } from "userservice";

//const capabilityService = new CapabilityService();

const app = new Vue({
    el: "#capabilitydetails-app",
    data: {
        items: [],
        membershipRequests: [],
        initializing: true,
        currentUser: currentUser
    },
    methods: {
        isCurrentUser: function(memberEmail) {
            return this.currentUser.email == memberEmail;
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
                .filter(member => member.email == this.currentUser.email)
                .length > 0;
        }
    },
    mounted: function () {
        this.initializing = false;
    }
});