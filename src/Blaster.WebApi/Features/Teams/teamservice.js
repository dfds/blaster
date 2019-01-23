import HttpClient from "httpclient";
import { currentUser } from "userservice";

export default class TeamService {
    constructor() {
        this.client = new HttpClient();
        this.baseUrl = "api/teams";

        this.getAll = this.getAll.bind(this);
        this.add = this.add.bind(this);
    }

    getAll() {
        return this.client.get(this.baseUrl)
            .then(data => data.items || []);
    }

    add (team) {
        return this.client.post(this.baseUrl, team);
    }

    join(teamId) {
        const payload = {
            email: currentUser.email
        };

        return this.client.post(`${this.baseUrl}/${teamId}/members`, payload);
    }

    leave(teamId) {
        return this.client.delete(`${this.baseUrl}/${teamId}/members/${currentUser.email}`);
    }
}