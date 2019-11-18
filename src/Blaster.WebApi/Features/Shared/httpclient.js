import jq from "jquery";

const baseUrl = `${window.basePath}`;
const axios = require('axios').default.create();

axios.interceptors.request.use(
    req => {
        const internalHttpClient = new HttpClient();
        console.log(req.url + " has been hit");
        if (internalHttpClient.authIsEndpointAuthed(req.url)) {
            console.log("Needs auth");
        }
        return req;
    },
    err => {
        console.log(err);
        return err;
    }
)

axios.interceptors.response.use(
    resp => {
        return resp;
    },
    err => {
        console.log(err);
        return err;
    }
)

export default class HttpClient {
    constructor() {
        this.createEndpointFrom = this.createEndpointFrom.bind(this);
        this.get = this.get.bind(this);
        this.post = this.post.bind(this);
        this.put = this.put.bind(this);
        this.delete = this.delete.bind(this);
        this.authEndpoints = new Map();
        this.init();
    }

    authAddEndpoint(endpoint) {
        this.authEndpoints.set(endpoint.value, endpoint);
    }

    // Not the most performant solution, but will suffice for now.
    authIsEndpointAuthed(endpoint) {
        for (let [key, val] of this.authEndpoints) {
            if (endpoint.includes(key)) {
                var reg = new RegExp(val.matchRegex);
                if (reg.test(endpoint)) {
                    return true;
                }
            }
        }
        return false;
    }

    createEndpointFrom(url) {
        let endpoint = url;

        if (!endpoint.startsWith("/")) {
            endpoint = "/" + endpoint;
        }

        return `${baseUrl}${endpoint}`;
    }

    get(url) {
        return axios.get(this.createEndpointFrom(url));
    }

    post(url, data) {
        return axios({
            url: this.createEndpointFrom(url),
            method: 'post',
            headers: {'Content-Type': 'application/json'},
            data: JSON.stringify(data)
        });
    }

    put(url, data) {
        return axios({
            url: this.createEndpointFrom(url),
            method: 'put',
            headers: {'Content-Type': 'application/json'},
            data: JSON.stringify(data)
        });
    }

    delete(url) {
        return axios.delete(this.createEndpointFrom(url));
    }

    init() {
        let capabilitiesEndpoint = new ProtectedEndpoint();
        capabilitiesEndpoint.value = "/api/capabilities";
        capabilitiesEndpoint.matchRegex = '\/api\/capabilities\/(.)\/?(.*)';
        this.authAddEndpoint(capabilitiesEndpoint);

        let connectionsEndpoint = new ProtectedEndpoint();
        connectionsEndpoint.value = "/api/connections";
        connectionsEndpoint.matchRegex = '\/api\/connections\/?';
        this.authAddEndpoint(connectionsEndpoint);
    }
}

export class ProtectedEndpoint {
    constructor() {
        this.value = "";
        this.matchRegex = "";
    }
}

export {HttpClient, axios}