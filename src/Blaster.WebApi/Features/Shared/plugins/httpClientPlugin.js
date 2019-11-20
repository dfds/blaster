import {HttpClient} from 'httpclient';

const HttpClientPlugin = {
    install(Vue, options) {
        const httpClient = new HttpClient();
        Vue.prototype.$http = httpClient;

        // Check if userManagementPlugin is loaded.
        if (Vue.prototype.isAuthenticated) {
            Vue.prototype.$http.setInterceptRequestHandler((req) => {
                console.log("Handling intercepted request");
                if (!Vue.prototype.isAuthenticated()) {
                    Vue.prototype.signIn();
                } else {
                    Vue.prototype.acquireToken(["user.read"])
                    .then(resp => {
                        console.log(resp);
                    });
                }
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