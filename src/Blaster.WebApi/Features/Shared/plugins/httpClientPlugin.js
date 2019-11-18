import {HttpClient} from 'httpclient';

const HttpClientPlugin = {
    install(Vue, options) {
        const httpClient = new HttpClient();
        Vue.prototype.$http = httpClient;

        // Check if userManagementPlugin is loaded.
        if (Vue.prototype.isAuthenticated) {
            Vue.prototype.$http.setInterceptRequestHandler((req) => {
                console.log("Handling intercepted request");
                console.log(req);
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