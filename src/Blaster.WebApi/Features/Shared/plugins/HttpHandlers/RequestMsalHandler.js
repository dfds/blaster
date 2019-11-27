
const HttpHeaderMsalAuthToken = "X-Msal-Auth-Token";
const RequestedScopes = ["api://24420be9-46e5-4584-acd7-64850d2f2a03/access_as_user"]; // Rewrite this to be more modular

async function RequestMsalHandler(req, Vue) {
  return new Promise((resolve, err) => {
    if (!Vue.prototype.isAuthenticated()) { // Not signed in
        let result = Vue.prototype.signIn();
        if (result) {
            result.then(() => {
                Vue.prototype.acquireToken(RequestedScopes)
                .then(resp => {
                    req.headers[HttpHeaderMsalAuthToken] = resp.accessToken;
                    resolve(req);
                });
            })
        }
    } else { // Already signed in
        Vue.prototype.getCachedAccessToken()
        .then(resp => {
            if (resp) {
                req.headers[HttpHeaderMsalAuthToken] = resp.accessToken;
                resolve(req);
            } else { // Acquire new token
                Vue.prototype.acquireToken(RequestedScopes)
                .then(resp => {
                    req.headers[HttpHeaderMsalAuthToken] = resp.accessToken;
                    resolve(req);
                });
            }
        });
    } 
});
}

export default function InstallRequestMsalHandler(vue) {
  if (vue.prototype.isAuthenticated) {
      vue.prototype.$http.setInterceptRequestHandler(async (req) => {
        return RequestMsalHandler(req, vue);
      });
  } else {
      throw new Error("Unable to install RequestMsalHandler, unable to find UserManagementPlugin.");
  }
}

export {RequestMsalHandler, InstallRequestMsalHandler};