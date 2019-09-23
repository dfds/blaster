import HttpClient from "httpclient";

export default class ChannelService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/connections";

        this.getAll = this.getAll.bind(this);
    }

    // Get public channels
    getAll() {
        return this.client.get(this.baseUrl + "s")
            .then(data => data.items || []);
    }

    getByCapabilityId(id) {
        return this.client.get(`${this.baseUrl}?senderId=${id}`)
            .then(data => data.items || []);
    }
}