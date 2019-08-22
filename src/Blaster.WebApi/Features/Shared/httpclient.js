import jq from "jquery";

const baseUrl = `${window.basePath}`;

export default class HttpClient {
    constructor() {
        this.createEndpointFrom = this.createEndpointFrom.bind(this);
        this.get = this.get.bind(this);
        this.post = this.post.bind(this);
        this.put = this.put.bind(this);
        this.delete = this.delete.bind(this);
    }

    createEndpointFrom(url) {
        let endpoint = url;

        if (!endpoint.startsWith("/")) {
            endpoint = "/" + endpoint;
        }

        return `${baseUrl}${endpoint}`;
    }

    get(url) {
        const endpoint = this.createEndpointFrom(url);
        return jq.getJSON(endpoint);
    }

    post(url, data) {
        return jq.ajax({
            type: "POST",
            url: this.createEndpointFrom(url),
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(data)
        });
    }

    put(url, data) {
        return jq.ajax({
            type: "PUT",
            url: this.createEndpointFrom(url),
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(data)
        });
    }

    delete(url) {
        return jq.ajax({
            type: "DELETE",
            url: this.createEndpointFrom(url)
        });
    }
}