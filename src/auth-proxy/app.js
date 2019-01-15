const proxy = require('express-http-proxy');
const app = require('express')();
const JWTUtilities = require('./jwt-utilities');
const jwtUtil = new JWTUtilities();
const port = process.env.PORT || 50800;
const forwardAddress = process.env.FORWARD_ADDRESS || "localhost:50801";

app.use('/', proxy(forwardAddress, {
    proxyReqOptDecorator: function(proxyReqOpts, srcReq) {
        
        var jwt = proxyReqOpts.headers["x-amzn-oidc-data"];
        var email = jwtUtil.getEmail(jwt);
        // console.log(email);
        
        proxyReqOpts.headers['X-Email'] = email;

        // console.log(JSON.stringify(proxyReqOpts));
        
        return proxyReqOpts;
    }})
);

app.listen(port, () => {
    console.log(`auth-proxy is running on port ${port}`);
});
