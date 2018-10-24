import jq from "jquery";
import Vue from "vue";
import "./styles";

const app = new Vue({
    el: "#aws-app",
    data: {
        awsLink: "http://no-link"
    }
});

function getAwsLink() {
    return jq.getJSON("api/teams/0f6beb61-0421-4b75-b226-3cce81061ca7/aws/console-url");
}

jq.ready
    .then(() => getAwsLink())
    .then(data => { 
        app.awsLink = data.absoluteUrl;
    });