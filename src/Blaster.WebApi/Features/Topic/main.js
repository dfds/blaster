import Vue from "vue";
import TopicService from "./topic-service";
import AlertDialog from "../Shared/alert-dialog";
import ModelEditor from "modeleditor";
import jq from "jquery";

const topicService = new TopicService();

const app = new Vue({
    el: "#topics-app",
    data: {
        items: [],
        initializing: true
    },
    computed: {
        hasTopics: function () {
            return this.items.length > 0;
        }
    },
    methods: {
        newTopic: function () {
            const editor = ModelEditor.open({
                template: document.getElementById("editor-template"),
                data: {
                    name: "",
                },
                onClose: () => editor.close(),
                onSave: (data) => {
                    return topicService.add(data)
                        .then(team => this.items.push(data.name))
                        .then(() => editor.close())
                        .catch(err => {
                            if (err.status == 400) {
                                const dialog = AlertDialog.open({
                                    template: document.getElementById("error-dialog-template"),
                                    container: jq(".dialog-container", editor.element),
                                    data: {
                                        title: "Validation issue",
                                        message: err.responseJSON.message
                                    }
                                });

                                setTimeout(function () {
                                    dialog.close();
                                }, 15000);
                            }
                            else if (err.status != 200) {
                                const dialog = AlertDialog.open({
                                    template: document.getElementById("error-dialog-template"),
                                    container: jq(".dialog-container", editor.element),
                                    data: {
                                        title: "Error!",
                                        message: `Unable to create topic. Server returned (${err.status}) ${err.statusText}.`
                                    }
                                });

                                setTimeout(function () {
                                    dialog.close();
                                }, 3000);
                            }
                        });
                }
            });
        }
    },
    mounted: function () {
        jq.ready
            .then(() => topicService.getAll())
            .then(topics => topics.forEach(topic => this.items.push(topic)))
            .catch(info => {
                if (info.status != 200) {
                    const message = `Could not retrieve list of capabilities. Server returned (${info.status}) ${info.statusText}.`;

                    AlertDialog.open({
                        template: document.getElementById("error-dialog-template"),
                        container: document.getElementById("global-dialog-container"),
                        data: {
                            title: "Error!",
                            message: message
                        }
                    });
                }
            })
            .done(() => this.initializing = false);
    }
});