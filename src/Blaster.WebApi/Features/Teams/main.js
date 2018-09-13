import jq from "jquery";
import Vue from "vue";
import Editor from "./team-editor";
import "./styles"

const app = new Vue({
    el: "#teams-app",
    data: {
        items: []
    },
    methods: {
        newTeam: function() {
            const item = {
                name: "",
                department: ""
            };

            const editor = new Editor();
            editor
                .open(item)
                .then(data => {
                    app.items.push({
                        name: data.name,
                        department: data.department,
                        members: ""
                    });
                });
        }
    }
});

function getTeams() {
    return jq.getJSON("api/teams");
    // return {
    //     items: [{
    //             name: "Team Foo",
    //             department: "lala",
    //             members: "foo, bar, baz, qux"
    //         },
    //         {
    //             name: "Team Bar",
    //             department: "asdfg",
    //             members: "1 2 3 4 5 6 7 8 9"
    //         }
    //     ]
    // };
}

jq.ready
    .then(() => getTeams())
    .then(data => {
        data.items.forEach(item => app.items.push(item));
    });