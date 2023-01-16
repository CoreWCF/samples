## Mixed-auth-allow-anonymous-workaround

This sample shows a mixed authentication service which uses two `ServiceContract` to require or not authenticated requests using two separate endpoints.
The pattern implemented can be used as a workaround to the lack of support of the `AllowAnonymous` attribute.
The authentication flow is known as [OAuth2.0 client_credentials](https://www.rfc-editor.org/rfc/rfc6749#section-1.3.4) flow. 

### Idp

`Idp` is a security token service which implements an OAuth client_credentials flow using [OpenIddict](https://github.com/openiddict).

### Service

The `/authenticated` endpoint of service `Service` is configured to accept requests authenticated with a valid bearer `access_token` issued by `Idp` with audience and scope valued to 'api'. The authentication is performed by the standard JwtBearer AuthenticationHandler shipped with ASP.NET Core in the `Microsoft.AspNetCore.Authentication.JwtBearer` nuget package.
The `/anonymous` endpoint of `Service` does not require authentication.

### Client

`Client` requests an `access_token` with the scope 'api' to the identity provider using its `client_id` and `client_secret`, then it calls the `/authenticated` `Service` and the `/anonymous` endpoints of `Service`.
