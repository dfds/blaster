import {HttpClient} from 'httpclient';

const HttpClientPlugin = {
    install(Vue, options) {
        const httpClient = new HttpClient();
        Vue.prototype.$http = httpClient;
    }
};

export default HttpClientPlugin;