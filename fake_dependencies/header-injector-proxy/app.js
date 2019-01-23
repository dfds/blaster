const proxy = require('express-http-proxy');
const app = require('express')();
const port = process.env.PORT || 50802;
const forwardAddress = process.env.FORWARD_ADDRESS || "localhost:50800";

app.use('/', proxy(forwardAddress, {
    proxyReqOptDecorator: function (proxyReqOpts, srcReq) {
        proxyReqOpts.headers['X-User-Id'] = base64Encode("jane@doe.com");
        proxyReqOpts.headers['X-User-Name'] = base64Encode("Jane Doe");
        proxyReqOpts.headers['X-User-Email'] = base64Encode("jane@doe.com");

        // Simulate real JWT-token:
        //proxyReqOpts.headers["X-Amzn-Oidc-Data"] = "";

        return proxyReqOpts;
    }})
);

app.listen(port, () => {
    console.log(`header-injector-proxy is running on port ${port}. Forwarding to ${forwardAddress}`);
});

function base64Encode(str, encoding = 'base64') {
    let buffer = Buffer.from(str);
    let text = buffer.toString(encoding);
    
    return text;
};