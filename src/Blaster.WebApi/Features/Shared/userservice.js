const currentUser = window.currentUser;

export { currentUser as currentUser }

export default class UserService {
    getCurrentUser() {
        return currentUser;
    }
}