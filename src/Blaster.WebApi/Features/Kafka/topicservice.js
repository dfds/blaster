import HttpClient from "httpclient";

export default class TopicService {
    constructor(httpClient) {
        this.client = httpClient ? httpClient : new HttpClient();
        this.baseUrl = "api/topics";
    }

    add(payload) {
        return this.client.post(this.baseUrl, payload).then(data => data || {});
    }

    getByCapabilityId(capabilityId) {
        return this.client.get(`api/capabilities/${capabilityId}/topics`)
            .then(response => response.data.items || []);
    }
}