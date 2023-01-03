## Minimal-machine-to-machine-using-JWT

This sample shows a minimal machine to machine authentication setup using JWT. This authentication is known as [OAuth2.0 client_credentials](https://www.rfc-editor.org/rfc/rfc6749#section-1.3.4) flow. The identity provider is [the demo instance of Duende IdentityServer](https://demo.duendesoftware.com) which provides configured OAuth2.0 clients.

### Service

`Service` is configured to accept requests authenticated with a valid bearer `access_token` issued by the https://demo.duendesoftware.com identity provider with audience and scope valued to 'api'. The authentication is performed by the standard JwtBearer AuthenticationHandler shipped with ASP.NET Core in the `Microsoft.AspNetCore.Authentication.JwtBearer` nuget package.

### Client

`Client` requests an `access_token` with the scope 'api' to the identity provider using its `client_id` and `client_secret`, then it calls the `Service` [passing its access_token in http headers](https://www.rfc-editor.org/rfc/rfc6749#section-7.1). 
```
Authorization: Bearer <access_token>
```