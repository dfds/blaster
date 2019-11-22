import * as Msal from "msal";

export default class UserService {
    constructor(configuration)
    {
        if (!configuration) {
            configuration = {
                auth: {
                    clientId: "91c38c20-4d2c-485d-80ac-a053619a02db",
                    authority: "https://login.microsoftonline.com/common",
                    redirectUri: "http://localhost:4200/login"
                },
                cache: {
                    cacheLocation: "localStorage",
                    storeAuthStateInCookie: true
                },
                request: {
                    scopes: ["user.read"]
                }
            };
        }

        if (!configuration.auth.clientId) {
            throw new Error('auth.clientId is required');
        }
        
        this.msalConfiguration = configuration;
        this.msalClient = new Msal.UserAgentApplication(this.msalConfiguration);

        this.data = {
            accessToken: "",
            //accessToken: {expiresOn: new Date()},
            user: this.msalClient.getAccount()
        };

        this.msalClient.handleRedirectCallback((error, response) => {
            this.data.user = response.account;
        });
    }

    getCurrentUser() {
        return (this.isAuthenticated()) ? this.data.user : window.currentUserRazor;
    }
    
    getCurrentUserEmail() {
        var user = this.getCurrentUser();

        return user.email || user.userName;
    }

    getCachedAccessToken() {
        return new Promise((resolve, reject) => {
            if (this.data.accessToken.expiresOn) {
                const expiredTime = this.data.accessToken.expiresOn.getTime();
                if (Date.now() < expiredTime) {
                    resolve(this.data.accessToken);
                } else {
                    resolve(undefined);
                }
            }
            resolve(undefined);
        });
    }

    isAuthenticated() {
        return !this.msalClient.isCallback(window.location.hash) && !!this.msalClient.getAccount();
    }

    signIn() {
        if (!this.isAuthenticated()) {
            var authPromise = (this.msalConfiguration.auth.redirectUri) ? this.msalClient.loginRedirect(this.msalConfiguration.request) : this.msalClient.loginPopup(this.msalConfiguration.request);

            if (authPromise) {
                authPromise.then(token => {
                    if (token.idTokenClaims.aud !== this.msalConfiguration.auth.clientId) {
                        throw new Error("The idToken received does not match the expected audience.");
                    }

                    this.data.user = token.account;
                });
                return authPromise;
            }
            else {
                this.data.user = this.msalClient.getAccount();
            }
        }
    }

    signOut() {
        if (this.isAuthenticated()) {
            this.msalClient.logout();
        }
    }

    acquireToken(scopes) {
        if (scopes) {
            scopes.authority = scopes.authority ? scopes.authority : this.msalConfiguration.auth.authority;
        }
        scopes = scopes || this.msalConfiguration.request.scopes;

        try {
            //Always start with acquireTokenSilent to obtain a token in the signed in user from cache.
            var accessToken = this.msalClient.acquireTokenSilent(scopes).then(accessToken => {
                this.data.accessToken = accessToken;
                return accessToken;
            });

            return accessToken;
        } catch (error) {
            // Upon acquireTokenSilent failure (due to consent or interaction or login required ONLY)
            // Call acquireTokenRedirect
            if (this.requiresInteraction(error.errorCode)) {
                (this.msalConfiguration.auth.redirectUri)
                    ? this.msalClient.acquireTokenRedirect(request)
                    : this.msalClient.acquireTokenPopup(request);
            }

            return false;
        }
    }

    requiresInteraction(errorCode)
    {
        if (!errorCode || !errorCode.length) {
            return false;
        }

        return errorCode === "consent_required" ||
            errorCode === "interaction_required" ||
            errorCode === "login_required";
    }
}

export {UserService}