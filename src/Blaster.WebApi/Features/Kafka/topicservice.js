import HttpClient from "httpclient";

export default class TopicService {
    constructor(httpClient) {
        this.client = httpClient ? httpClient : new HttpClient();
        this.baseUrl = "api/topics";
    }

    add(capabilityId, payload) {
        return this.client
			.post(`api/capabilities/${capabilityId}/topics`, payload)
			.then(response => response.data || {});
    }

    getByCapabilityId(capabilityId) {
        return this.client
			.get(`api/capabilities/${capabilityId}/topics`)
            .then(response => response.data.items || []);
    }
}