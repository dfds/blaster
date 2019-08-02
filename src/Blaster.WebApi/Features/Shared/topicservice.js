import HttpClient from "httpclient";

export default class TopicService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/topics";

        this.getAll = this.getAll.bind(this);
    }

    add(payload) {
        return this.client.post(this.baseUrl, payload).then(data => data || {});
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