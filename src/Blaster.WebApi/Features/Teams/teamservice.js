import jq from "jquery";

const baseUrl = "api/teams"

export default class TeamService {
    getAll() {
        return jq.getJSON(baseUrl);
    }

    add (team) {
        return jq.ajax({
            type: "POST",
            url: baseUrl,
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(team)
        });
    }    
}