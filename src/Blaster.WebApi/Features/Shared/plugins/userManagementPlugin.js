import Vue from "vue";
//import msal from 'vue-msal';
import * as UserService from "userservice";

//Vue.use(msal, {
//    auth: {
//        clientId: '91c38c20-4d2c-485d-80ac-a053619a02db',
//        requireAuthOnInitialize: false,
//        redirectUri: "http://localhost:5000/"
//    }
//});

const UserManagementPlugin = {
    install(Vue, options) {
        console.log('installing plugin');

        var us = new UserService.default();

        Vue.prototype.getUser = function () {
            //if (Object.keys(this.$msal.data.user).length > 0) {
            //    return this.$msal.data.user;
            //}

            return us.getCurrentUser();
        }

        Vue.prototype.getUserName = function() {
            var user = this.getUser();

            return user.email; // || user.name.split(' ').reduce((a, b) => b + ' ' + a);
        }

        Vue.prototype.isAuthenticated = function () {
            console.log(us);
            return false; //this.$msal.isAuthenticated();
        }

        Vue.prototype.signIn = function () {
            console.log('ump.signIn');
            us.signIn();
            /*if (!this.$msal.isAuthenticated()) {
                this.$msal.signIn();
            }*/
        }

        Vue.prototype.signOut = function () {
            us.signOut;
            /*if (this.$msal.isAuthenticated()) {
                this.$msal.signOut();
            }*/
        }
    }
};

export default UserManagementPlugin;