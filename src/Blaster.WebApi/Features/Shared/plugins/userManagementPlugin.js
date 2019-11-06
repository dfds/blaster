import * as UserService from "userservice";

const UserManagementPlugin = {
    install(Vue, options) {
        var userService = new UserService.default();

        Vue.prototype.getUser = function () {
            return userService.getCurrentUser();
        }

        Vue.prototype.getUserName = function () {
            var user = this.getUser();
            
            return (user) ? user.email || user.name.split(' ').reduce((a, b) => b + ' ' + a) : "guest";
        }

        Vue.prototype.isAuthenticated = function () {
            return userService.isAuthenticated();
        }

        Vue.prototype.signIn = function () {
            if (!userService.isAuthenticated()) {
                userService.signIn();
            }
        }

        Vue.prototype.signOut = function () {
            if (userService.isAuthenticated()) {
                userService.signOut();
            }
        }

        Vue.prototype.acquireToken = function (scopes) {
            if (userService.isAuthenticated()) {
                userService.acquireToken({ scopes: scopes });
            }
        }
    }
};

export default UserManagementPlugin;