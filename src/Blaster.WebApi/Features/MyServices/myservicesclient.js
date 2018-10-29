import HttpClient from "httpclient";
import {currentUser} from "userservice";
import jq from "jquery";

export default class MyServicesClient {
    constructor() {
        this.httpClient = new HttpClient();
        this.baseUrl = "api/users/";

        this.goToService = this.goToService.bind(this);
        this.getAll = this.getAll.bind(this);
    }

    goToService(url) {
        if (url.includes('console-url') == false) {
            window.location.href = url;
        } else {
            jq.getJSON(url, function (data) {
                window.location.href = data.absoluteUrl;
            });
        }
    }

    getAll() {
        return this.httpClient.get(this.baseUrl + currentUser.id + "/services");
    }
}