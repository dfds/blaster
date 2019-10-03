import "core-js/features/url-search-params"
import "core-js/stable";
import "regenerator-runtime/runtime";
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

    // The API contract is in a state of flux. This endpoint may be removed at some point.
    join(payload) {
        return this.client.post(`${this.baseUrl}`, payload);
    }

    // The API contract is in a state of flux. This endpoint may be removed at some point.
    leave(payload) {
        var searchParams = new URLSearchParams("");
        if (payload.clientType) {
            searchParams.append("clientType", payload.clientType);
        }
        if (payload.clientId) {
            searchParams.append("clientId", payload.clientId);
        }
        if (payload.channelType) {
            searchParams.append("channelType", payload.channelType);
        }
        if (payload.channelId) {
            searchParams.append("channelId", payload.channelId);
        }

        return this.client.delete(`${this.baseUrl}?${searchParams.toString()}`);
    }
}