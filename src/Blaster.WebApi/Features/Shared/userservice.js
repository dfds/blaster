const currentUser = window.currentUser;

export { currentUser as currentUser }

export default class UserService {
    getCurrentUser() {
        return this.msal.account.userName ||  currentUser;
    }
}