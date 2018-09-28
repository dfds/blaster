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
        return this.client.get(this.baseUrl);
    }

    add (team) {
        return this.client.post(this.baseUrl, team);
    }

    join(teamId) {
        const payload = {
            userId: currentUser.id
        };

        return this.client.post(`/api/teams/${teamId}/members`, payload);
    }
}