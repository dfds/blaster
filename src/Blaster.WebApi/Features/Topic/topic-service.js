import HttpClient from "httpclient";

export default class TopicService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/topics";

        this.getAll = this.getAll.bind(this);
        this.add = this.add.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.items || []);
    }

    add (topic) {
        return this.client.post(this.baseUrl, topic);
    }
}