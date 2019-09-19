import HttpClient from "httpclient";

export default class ChannelService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/channels";

        this.getAll = this.getAll.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.items || []);
    }
}