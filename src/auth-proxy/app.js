const proxy = require('express-http-proxy');
const app = require('express')();
const port = process.env.PORT || 50800;
const forwardAddress = process.env.FORWARD_ADDRESS || "localhost:50801";

app.use('/', proxy(forwardAddress, {
    proxyReqOptDecorator: function(proxyReqOpts, srcReq) {
        
        var jwt = proxyReqOpts.headers["x-amzn-oidc-data"];
        var email = getEmail(jwt);
        // console.log(email);
        
        proxyReqOpts.headers['X-Email'] = email;

        // console.log(JSON.stringify(proxyReqOpts));
        
        return proxyReqOpts;
    }})
);

app.listen(port, () => {
    console.log(`auth-proxy is running on port ${port}`);
});

function getEmail(jwt) {
    if (jwt == undefined) {
        return "";
    }

    var jwtParts = jwt.split(".");

    if (jwtParts.length != 3) {
        return "";
    }

    var userPartBase64 = jwtParts[1];
    var userPart = JSON.parse(base64Decode(userPartBase64));
    
    return userPart.email;
}

function base64Decode(str, encoding = 'utf-8') {
    let buff = Buffer.from(str, 'base64');  
    let text = buff.toString(encoding);
    
    return text;
}