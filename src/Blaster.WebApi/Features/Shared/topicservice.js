import HttpClient from "httpclient";

export default class TopicService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/topics";

        this.getAll = this.getAll.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.items || []);
    }

    getById(topicId) {
        return this.client.get(`${this.baseUrl}/${topicId}`)
            .then(data => data || {});
    }

    getByCapabilityId(capabilityId) {
        return this.client.get(`${this.baseUrl}/by-capability-id/${capabilityId}`)
            .then(data => data.items || []);
    }
}