import moment from "moment";
import jq from "jquery";
import Mustache from "Mustache";
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
        openEditor: function(item) {

            jq.getJSON(`api/dashboards/${item.id}`)
                .then(data => {
                    const editorTemplate = jq("#editor-template").html();
                    return Mustache.render(editorTemplate, Object.assign(item, { content: data.content }));
                })
                .then(editorMarkup => {
                    const element = jq(editorMarkup); 
                    jq(document.body).append(element);

                    return new Promise((resolve, reject) => {
                        jq("[data-behavior=close]", element).click(() => {
                            reject();
                            element.remove();
                        });
                        jq("[data-behavior=save]", element).click(() => {
                            resolve({
                                name: jq("[data-property=name]", element).val(),
                                team: jq("[data-property=team]", element).val(),
                                content: jq("[data-property=content]", element).val()
                            });
                            element.remove();
                        });
                    });
                })
                .then(data => {
                    item.name = data.name;
                    item.team = data.team;
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