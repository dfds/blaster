import HttpClient from "httpclient";

export default class AWSPermissionsService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/awspermissions";

        this.getAll = this.getAll.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.items || []);
    }
}