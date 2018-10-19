import HttpClient from "httpclient";
import { currentUser } from "userservice";

export default class MyServicesClient {
    constructor() {
        this.httpClient = new HttpClient();
        this.baseUrl = "api/myservices";

        this.getAll = this.getAll.bind(this);
    }


    getAll() {
        return this.httpClient.get(this.baseUrl);
    }
}