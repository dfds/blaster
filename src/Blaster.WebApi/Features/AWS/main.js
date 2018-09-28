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
    return jq.getJSON("api/aws");
}

jq.ready
    .then(() => getAwsLink())
    .then(data => { 
        app.awsLink = data.absoluteUrl;
    });