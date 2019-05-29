import HttpClient from "httpclient";

export default class TopicService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/topics";

        this.topics = [{
            "id": require('crypto').randomBytes(16).toString('hex'), 
            "name":"aaa", 
            "description": "description", 
            "visibility": "private", 
            "messageExamples" :[
                {
                    "id": require('crypto').randomBytes(16).toString('hex'),
                    "messageType" : "capability_created",
                    "text" : "\{ \"messageType\" : \"capability_created\"\}"
                }
            ]
        }]; 
    }
    getByCapabilityId(capabilityId) {
       return this.topics;
    }

    get(topicId){
        return this.client.get(`${this.baseUrl}/${topicId}`)
        .then(data => data || {});
    }
  
    addMessageExample(topicId, messageExample){
        let topic = this.topics.find(obj => {
            return obj.id === topicId
        });
  
        messageExample.id = require('crypto').randomBytes(16).toString('hex');
        topic.messageExamples.push(messageExample);

        return new Promise((resolve, reject) => resolve());
    }

    add(topic){
        topic.id = require('crypto').randomBytes(16).toString('hex'),
        topic.messageExamples = [];
        this.topics.push(topic);

        return new Promise((resolve, reject) => resolve(topic));
    }
}