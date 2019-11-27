import {HttpClient} from 'httpclient';

const HttpClientPlugin = {
    install(Vue, options) {
        const httpClient = new HttpClient();
        Vue.prototype.$http = httpClient;

        if (options) {
            if (options.interceptRequestHandler) {
                Vue.prototype.$http.setInterceptRequestHandler(options.interceptRequestHandler);
            }
        }
    }
};

export default HttpClientPlugin;