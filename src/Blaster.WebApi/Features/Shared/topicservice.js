import HttpClient from "httpclient";

export default class TopicService {
    constructor(httpClient) {
        this.client = httpClient ? httpClient : new HttpClient();
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

    update(topicId, payload) {
        return this.client.put(`${this.baseUrl}/${topicId}`, payload);
    }

    getMessageContractsByTopicId(topicId) {
        return this.client.get(`${this.baseUrl}/${topicId}/messageContracts`)
            .then(data => data.items || []);
    }

    // TODO: Made obsolete in recent API contract revision, to be removed.
    addMessageContract(topicId, payload) {
        return this.client.post(`${this.baseUrl}/${topicId}/messageContracts`, payload);
    }

    addOrUpdateMessageContract(topicId, mcType, payload) {
        return this.client.put(`${this.baseUrl}/${topicId}/messageContracts/${mcType}`, payload);
    }

    deleteMessageContract(topicId, mcType) {
        return this.client.delete(`${this.baseUrl}/${topicId}/messageContracts/${mcType}`);
    }
}