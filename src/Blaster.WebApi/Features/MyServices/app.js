import Vue from 'vue';
import { currentUser } from "userservice";
import MyServicesClient from "./myservicesclient";

const myServicesClient = new MyServicesClient();

const app = new Vue({
    el: '#myservices-app',
    data: {
        initializing: true,
        teams: [],
        currentUser: currentUser
    },
    methods: {
        goToService: function (url) {
            myServicesClient.goToService(url);
        }
    },
    created(){
        myServicesClient
            .getAll()
            .then(data => {
                const items = data.items || [];
                app.teams = items;
            })
            .then(() => app.initializing = false);
    }
});