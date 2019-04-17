import HttpClient from "httpclient";
import { currentUser } from "userservice";

export default class CapabilityService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/capabilitiesv2";

        this.getAll = this.getAll.bind(this);
        this.add = this.add.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.items || []);
    }

    add (capability) {
        return this.client.post(this.baseUrl, capability);
    }

    join(capabilityId) {
        const payload = {
            email: currentUser.email
        };

        return this.client.post(`${this.baseUrl}/${capabilityId}/members`, payload);
    }

    leave(capabilityId) {
        return this.client.delete(`${this.baseUrl}/${capabilityId}/members/${currentUser.email}`);
    }
}