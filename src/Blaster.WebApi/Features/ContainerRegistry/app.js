import Vue from "vue";
import ModelEditor from "modeleditor";
import { service as repositoryService } from "./containerrepositoryservice";
import AlertDialog from "alertdialog";
import jq from "jquery";

function repositoryComparer(left, right) {
    return left.name.localeCompare(right.name);
}

export default new Vue({
    el: "#containerregistry-app",
    data: {
        repositories: [],
        searchText: null,
        appStatus: "loading"
    },
    computed: {
        hasRepositories: function() {
            return this.repositories && this.repositories.length > 0;
        },
        searchResult: function() {
            if (!this.searchText) {
                return this.repositories;
            }

            return this.repositories.filter(x => x.name.indexOf(this.searchText) > -1);
        },
        isRunning: function() {
            return this.appStatus == "running";
        },
        isLoading: function() {
            return this.appStatus == "loading";
        }
    },
    methods: {
        setStatus: function(newStatus) {
            this.appStatus = newStatus || "error";
        },
        search: function (event) {
            const term = event.target.value;
            this.searchText = term;
        },
        setRepositories: function(repositories) {
            const repos = repositories || [];
            repos.sort(repositoryComparer);
            this.repositories = repos;
        },
        addRepository: function(newRepository) {
            const repos = this.repositories || [];
            repos.push(newRepository);
            this.setRepositories(repos);
        },
        add: function() {
            const self = this;
            const editor = ModelEditor.open({
                template: document.getElementById("editor-template"),
                data: {
                    name: ""
                },
                onClose: () => editor.close(),
                onSave: (repositoryData) => {
                    repositoryService
                        .add(repositoryData)
                        .then(newRepository => self.addRepository(newRepository))
                        .then(() => editor.close())
                        .catch(err => {
                            if (err.status != 200) {
                                AlertDialog.open({
                                    template: document.getElementById("error-dialog-template"),
                                    container: jq(".dialog-container", editor.element),
                                    autoClose: 3000,
                                    data: {
                                        title: "Error!",
                                        message: `Unable to save container repository. Server returned (${err.status}) ${err.statusText}.`
                                    }
                                });
                            }
                        });
                }
            })
        }
    }
});