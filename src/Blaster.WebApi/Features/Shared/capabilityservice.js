import HttpClient from "httpclient";
import * as UserService from "userservice";

export default class CapabilityService {
    constructor(httpClient, userService) {
        this.client = httpClient ? httpClient : new HttpClient();
        this.baseUrl = "api/capabilities";

        this.getAll = this.getAll.bind(this);
        this.add = this.add.bind(this);
        this.userService = userService ? userService : new UserService.default();
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.data || []);
    }

    get(capabilityId) {
        return this.client.get(`${this.baseUrl}/${capabilityId}`)
            .then(data => data.data || {});
    }

    add (capability) {
        return this.client.post(this.baseUrl, capability);
    }

    update(capabilityId, capability) {
        return this.client.put(`${this.baseUrl}/${capabilityId}`, capability);
    }

    delete(capabilityId) {
        return this.client.delete(`${this.baseUrl}/${capabilityId}`);
    }

    join(capabilityId) {
        const payload = {
            email: this.userService.getCurrentUserEmail()
    };

        return this.client.post(`${this.baseUrl}/${capabilityId}/members`, payload);
    }

    addContext(capabilityId) {
        return this.client.post(`${this.baseUrl}/${capabilityId}/contexts`);
    }

    addTopic(payload, capabilityId) {
        return this.client.post(`${this.baseUrl}/${capabilityId}/topics`, payload);
    }

    setCommonPrefix(payload, capabilityId) {
        return this.client.post(`${this.baseUrl}/${capabilityId}/commonprefix`, payload);
    }

    leave(capabilityId) {
        return this.client.delete(`${this.baseUrl}/${capabilityId}/members/${this.userService.getCurrentUserEmail()}`);
    }
}