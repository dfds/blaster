import HttpClient from "httpclient";

export default class AWSPermissionsService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/awspermissions";

        this.getAllByCapability = this.getAllByCapability.bind(this);
    }

    getAllByCapability(capabilityId) {
        return this.client.get(`${this.baseUrl}/${capabilityId}`)
            .then(data => data.items || []);
    }
}