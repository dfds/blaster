import HttpClient from "httpclient";

export default class ConnectionsService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/capabilities";

        this.get = this.getByCapabilityId.bind(this);
    }

    getByCapabilityId(capabilityId){
        return this.client
        .get(`${this.baseUrl}/${capabilityId}/connections`)
        .then(data => data.items || []);
    }
}