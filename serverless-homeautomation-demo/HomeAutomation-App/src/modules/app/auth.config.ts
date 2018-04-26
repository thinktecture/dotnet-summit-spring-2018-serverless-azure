import { AuthConfig } from 'angular-oauth2-oidc';

export const resourceOwnerConfig: AuthConfig = {
  issuer: 'https://tt-identityserver4-demo.azurewebsites.net',
  clientId: 'resourceowner',
  dummyClientSecret: 'no-really-a-secret',
  scope: 'openid profile email api',
  oidc: false
}
