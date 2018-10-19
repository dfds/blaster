import Vue from 'vue';
import { currentUser } from "userservice";
import MyServicesClient from "./myservicesclient";

const myServicesClient = new MyServicesClient();

const app = new Vue({
    el: '#myservices-app',
    data: {
        teams: [],
        currentUser: currentUser
    },
    created(){
        myServicesClient
            .getAll()
            .then(data => {
                const items = data.items || [];
                app.teams = items;
            });
    }
});