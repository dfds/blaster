const proxy = require('express-http-proxy');
const app = require('express')();
const JWTUtilities = require('./jwt-utilities');
const jwtUtil = new JWTUtilities();
const port = process.env.PORT || 50800;
const forwardAddress = process.env.FORWARD_ADDRESS || "localhost:50801";

app.use('/', proxy(forwardAddress, {
    proxyReqOptDecorator: function(proxyReqOpts, srcReq) {
        console.log("Looking for x-amzn-oidc-data");
        let jwt = proxyReqOpts.headers["x-amzn-oidc-data"];
        proxyReqOpts.headers.forEach(function(item, index, array) {
            console.log(item, index);
          });
        console.log(jwt);

        let user = jwtUtil.getUserInformation(jwt);
        
        if (user != null) {
            proxyReqOpts.headers['X-User-Id'] = jwtUtil.base64Encode(user.id);
            proxyReqOpts.headers['X-User-Name'] = jwtUtil.base64Encode(user.name);
            proxyReqOpts.headers['X-User-Email'] = jwtUtil.base64Encode(user.email);
        }
        
        return proxyReqOpts;
    }})
);

app.listen(port, () => {
    console.log(`auth-proxy is running on port ${port}`);
});
