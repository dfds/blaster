import * as Msal from "msal";

export default class UserService {
    constructor(configuration)
    {
        if (!configuration) {
            configuration = {
                auth: {
                    clientId: "91c38c20-4d2c-485d-80ac-a053619a02db",
                    authority: "https://login.microsoftonline.com/73a99466-ad05-4221-9f90-e7142aa2f6c1",
                    redirectUri: location.protocol + "//" + location.host + "/login"
                },
                cache: {
                    cacheLocation: "localStorage",
                    storeAuthStateInCookie: false
                },
                request: {
                    scopes: ["user.read", "offline_access", "openid"]
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
        var email = user.email || user.userName;

        return email.toLowerCase();
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

        var accessTokenPromise = this.msalClient.acquireTokenSilent(scopes).then(accessToken => {
	        this.data.accessToken = accessToken;

	        return accessToken;
        }).catch(err => {
            if (this.requiresInteraction(err.errorCode)) {
                if (this.msalConfiguration.auth.redirectUri) {
                    this.msalClient.acquireTokenRedirect(scopes);
                    return new Promise(function (resolve) { resolve({accessToken: ""})});
                }
                else {
                    return this.msalClient.acquireTokenPopup(scopes).then(response => {
			            return response.accessToken;
		            });
	            }
	        }
        });

        return accessTokenPromise;
    }

    requiresInteraction(errorCode) {
        if (!errorCode || !errorCode.length) {
            return false;
        }

        return errorCode === "consent_required" ||
            errorCode === "interaction_required" ||
            errorCode === "login_required";
    }
}

export {UserService}