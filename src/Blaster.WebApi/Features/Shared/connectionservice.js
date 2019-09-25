import HttpClient from "httpclient";

export default class ConnectionService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/connections";

        this.getAll = this.getAll.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl + "s")
            .then(data => data.items || []);
    }

    getByCapabilityId(id) {
        return this.client.get(`/api/capabilities/${id}/connections`)
            .then(data => data.items || []);
    }
}