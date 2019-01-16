const currentUser = window.currentUser || { email: "jdog@me.com"};

export { currentUser as currentUser }

export default class UserService {
    getCurrentUser() {
        return currentUser;
    }
}