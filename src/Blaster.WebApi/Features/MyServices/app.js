import Vue from 'vue';

const app = new Vue({
    el: '#myservices-app',
    data: {
        teams: [
            {
                name: 'awesome',
                availableServices: ['aws console', 'slack channel']
            },
            {name: 'the unnamed team that takes up a lot of space'}
        ]
    }
});