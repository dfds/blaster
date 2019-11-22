import {HttpClient} from 'httpclient';

const HttpClientPlugin = {
    install(Vue, options) {
        const httpClient = new HttpClient();
        Vue.prototype.$http = httpClient;

        // Check if userManagementPlugin is loaded.
        if (Vue.prototype.isAuthenticated) {
            Vue.prototype.$http.setInterceptRequestHandler(async (req) => {
                return new Promise((resolve, err) => {
                    console.log("Handling intercepted request");
                    if (!Vue.prototype.isAuthenticated()) { // Not signed in
                        let result = Vue.prototype.signIn();
                        if (result) {
                            result.then(() => {
                                console.log("After signIn acquiring token");
                                Vue.prototype.acquireToken(["user.read"])
                                .then(resp => {
                                    console.log(resp);
                                    req.headers["X-Msal-Auth-Token"] = resp.accessToken;
                                    resolve(req)
                                });
                            })
                        }
                    } else { // Already signed in
                        console.log("Already signed in, acquiring token.")
                        Vue.prototype.getCachedAccessToken()
                        .then(resp => {
                            if (resp) {
                                console.log("Using cached token");
                                console.log(resp);
                                req.headers["X-Msal-Auth-Token"] = resp.accessToken;
                                resolve(req)
                            } else { // Acquire new token
                                console.log("Acquiring new token");
                                Vue.prototype.acquireToken(["user.read"])
                                .then(resp => {
                                    console.log(resp);
                                    req.headers["X-Msal-Auth-Token"] = resp.accessToken;
                                    resolve(req)
                                });
                            }
                        });
                    } 
                });
            });

            if (options) {
                if (options.interceptRequestHandler) {
                    Vue.prototype.$http.setInterceptRequestHandler(options.interceptRequestHandler);
                }
            }
        }
    }
};

export default HttpClientPlugin;