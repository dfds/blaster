
export default class TopicService {
    constructor(){
        this.topics = [{"id": require('crypto').randomBytes(16).toString('hex'), "name":"aaa", "visibility": "private"}]; 
    }

    get(capabilityId) {
       return this.topics;
    }

    add(topic){
        topic.id = require('crypto').randomBytes(16).toString('hex'),

        this.topics.push(topic);

        return  new Promise((resolve, reject) => resolve(topic));
    }
}