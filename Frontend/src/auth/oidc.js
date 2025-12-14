import { UserManager } from "oidc-client-ts";

export const oidcConfig = {
  authority: "https://localhost:7133",   // YOUR OAuth server
  client_id: "spa-client",
  redirect_uri: "http://localhost:8080/callback",
  post_logout_redirect_uri: "http://localhost:8080/",
  response_type: "code",
  scope: "profile email",
  automaticSilentRenew: false,
  loadUserInfo: true
};

export const userManager = new UserManager(oidcConfig);

// login
export function login() {
  var response = userManager.signinRedirect();
  console.log(response);
  return response;
}

// logout
export function logout() {
  return userManager.signoutRedirect();
}

