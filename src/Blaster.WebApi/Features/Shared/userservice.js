import { UserAgentApplication } from "msal";

export default class UserService extends UserAgentApplication {
    constructor(configuration)
    {
        if (!configuration) {
            configuration = {
                auth: {
                    clientId: "91c38c20-4d2c-485d-80ac-a053619a02db",
                    authority: "https://login.microsoftonline.com/common",
                    redirectUri: "http://localhost:5000/"
                }
            };
        }

        if (!configuration.auth.clientId) {
            throw new Error('auth.clientId is required');
        }

        super(configuration);

        this.handleRedirectCallback((error, response) => {
            // handle redirect response or error
            console.log("redirect callback", error, response);
        });

        this.request = { scopes: ["user.read"]};

        this.data = {
            isAuthenticated: false,
            accessToken: '',
            user: {}
        };
    }

    getCurrentUser() {
        return window.currentUserRazor || ;
    }

    signIn() {
        console.log('us.signin');

        if (!this.isCallback(window.location.hash) && !this.getAccount()) {
            this.loginRedirect(this.request);
        }
    }

    signOut() {
        this.logout();
    }
}