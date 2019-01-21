const proxy = require('express-http-proxy');
const app = require('express')();
const port = process.env.PORT || 50802;
const forwardAddress = process.env.FORWARD_ADDRESS || "localhost:50800";

app.use('/', proxy(forwardAddress, {
    proxyReqOptDecorator: function (proxyReqOpts, srcReq) {
        proxyReqOpts.headers['X-User-Id'] = "jane@doe.com";
        proxyReqOpts.headers['X-User-Name'] = "Jane Doe";
        proxyReqOpts.headers['X-User-Email'] = "jane@doe.com";

        return proxyReqOpts;
    }})
);

app.listen(port, () => {
    console.log(`header-injector-proxy is running on port ${port}. Forwarding to ${forwardAddress}`);
});
