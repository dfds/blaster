const proxy = require('express-http-proxy');
const app = require('express')();
const JWTUtilities = require('./jwt-utilities');
const jwtUtil = new JWTUtilities();
const port = process.env.PORT || 50800;
const forwardAddress = process.env.FORWARD_ADDRESS || "localhost:50801";

app.use('/', proxy(forwardAddress, {
    proxyReqOptDecorator: function(proxyReqOpts, srcReq) {
        let jwt = proxyReqOpts.headers["x-amzn-oidc-data"];
        let user = jwtUtil.getUserInformation(jwt);
        
        proxyReqOpts.headers['X-User-Id'] = user.id;
        proxyReqOpts.headers['X-User-Name'] = user.name;
        proxyReqOpts.headers['X-User-Email'] = user.email;
        
        return proxyReqOpts;
    }})
);

app.listen(port, () => {
    console.log(`auth-proxy is running on port ${port}`);
});
