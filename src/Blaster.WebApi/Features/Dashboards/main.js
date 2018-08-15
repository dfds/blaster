import moment from "moment";
import jq from "jquery";
import DashboardEditor from "./dashboard-editor.js";
import "./styles.scss";

const app = new Vue({
    el: '#app',
    data: {
        items: []
    },
    computed: {
        totalCount: function() {
            return this.items.length;
        }
    },
    methods: {
        newItem: function() {
            const item = {
                name: "",
                team: "",
                content: ""
            };

            const editor = new DashboardEditor();
            editor.open(item)
                .then(item => {
                    return jq.ajax({
                        type: "POST",
                        url: "api/dashboards",
                        dataType: "application/json",
                        data: item
                    });
                })
                .then(data => {
                    console.log("SUCCESS!!: " + JSON.stringify(data));
                    return data;
                })
                .catch(info => {
                    console.log("OHNO!! " + JSON.stringify(info));
                });
        },
        openEditor: function(item) {
            jq.getJSON(`api/dashboards/${item.id}`)
                .then(data  => {
                    return Object.assign(item, { 
                        content: data.content 
                    });
                })
                .then(dataItem => {
                    const editor = new DashboardEditor();
                    return editor.open(dataItem);
                })
                .then(dataItem => {
                    item.name = dataItem.name;
                    item.team = dataItem.team;
                });
        }
    }
});

jq.ready
    .then(() => jq.getJSON("api/dashboards"))
    .then(data => {
        data.items.forEach(item => {
            item.lastModified = moment(item.lastModified).calendar();
            app.items.push(item);
        });
    });