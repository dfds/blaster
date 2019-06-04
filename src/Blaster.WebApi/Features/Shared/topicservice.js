import HttpClient from "httpclient";

export default class TopicService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/topics";
    }

    get(topicName){
        return this.client.get(`${this.baseUrl}/${topicName}`)
        .then(data => data || {});
    }
  
    addMessageExample(topicName, messageExample){
        return this.client.post(`${this.baseUrl}/${topicName}/messageexamples`, messageExample);
    }

    add(topic){
        topic.messageExamples = [];
        this.topics.push(topic);

        return new Promise((resolve, reject) => resolve(topic));
    }
}