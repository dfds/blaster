import HttpClient from "httpclient";

export default class ChannelService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/channel";

        this.getAll = this.getAll.bind(this);
    }

    // Get public channels
    getAll() {
        return this.client.get(this.baseUrl + "s")
            .then(data => data.data.items || []);
    }

    // The API contract is in a state of flux. This endpoint may be removed at some point.
    join(payload) {
        return this.client.post(`${this.baseUrl}`, payload);
    }

    // The API contract is in a state of flux. This endpoint may be removed at some point.
    leave(payload) {
        return this.client.post(`${this.baseUrl}/leave`, payload);
    }
}