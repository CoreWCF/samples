# Service-to-Service Authentication Using a JWT

## Introduction

This sample shows how a client can authenticate with a CoreWCF service using JWT-based authentication. It includes two services; one that uses Azure Active Directory as the identity provider, and another that makes authenticated requests to the first service. The same pattern will apply to other JWT-based authentication providers or to end-user rather than service-to-service authentication.

## What is a JWT

[JSON Web Token](https://en.wikipedia.org/wiki/JSON_Web_Token) is a way to support federated authentication and authorization of http requests. The client makes a call to an authentication server passing credentials and identifying information about the resource they wish to access. If authentication succeeds and the resource is authorized, they are handed back a signed token which includes information about the resource and rights. The client then passes that token to the service, which can then verify its integrity and trust the claims it specifies.

JWT is an open standard supported by multiple server stacks, clients, code languages and identity providers.

## Azure Active Directory

This particular sample is designed for use with Azure Active Directory (AAD) as the identity provider. However, the code that is specific to AAD is limited and another identity provider can easily be handled by replacing:
- In the client, the code and settings to authenticate with AAD and retrieve the token
- In the server, the code and settings to use AAD as the JWT provider for ASP.NET Core

If you wish to run the samples as-is, you will need to register the applications with an existing Azure AD tenant or create a new one. This sample is based on one of the [Azure AD samples](https://github.com/Azure-Samples/active-directory-dotnetcore-daemon-v2/tree/master/4-Call-OwnApi-Pop). Instructions on how to register apps are included in its README.

## Projects

The sample consists of 2 projects around the concept of a word game:
- TileService is a WCF Core server app that exposes an http endpoint that will return a set of letter tiles that could be used in a word game. 
- WordGame is an ASP.NET Core Web API endpoint that will call the TileService and return the results as a JSON blob.

These projects were chosen so that there is minimal sample code that is not related to the role of the authentication flow.

## Managing Secrets

In any kind of scenario involving service-to-service authentication, you will invariably need to deal with some form of shared secret, be it a string or a client certificate. These should **NOT** be included in code or be added to source control. At runtime, the secrets should come from some form of secure storage. .NET makes this easier to manage through the configuration API and built-in overlays:
- At design time, the [User Secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets) feature will create a configuration overlay file that can be used to store secrets. In Visual Studio, right-click on the project and choose *Manage User Secrets* to create and open the overlay file. Client Secrets should be stored in this file during development.
- At runtime, environment variables will overlay config properties. Connection strings and secrets can be placed there and then accessed via configuration. Hosting environments have support for safely storing secrets and supplying them at runtime. For instance, here are instructions for doing this in [Azure App Service](https://docs.microsoft.com/azure/app-service/configure-common?tabs=portal#configure-app-settings) or [Azure Container Apps](https://docs.microsoft.com/azure/container-apps/manage-secrets?tabs=azure-cli)

The appsettings.json files in the projects include the names of the config parameters that are required for the AAD authentication. They can be stored in User Secrets for additional security during development.

# Using JWT with WCF Services

The WS-* specifications which define the SOAP protocol and form the basis for WCF were developed long before JWT came onto the scene as the preferred form of web authentication. For this reason the WCF client APIs don't include direct support for JWT-based authentication or authorization. However, JWT is implemented over http by supplying the token as a base64-encoded string as the `Authorization` header. These samples add that header and validate it as part of the service call.

## Azure AD configuration
Within the Azure AD tenant used for authentication, the following need to be configured. See this [README](https://github.com/Azure-Samples/active-directory-dotnetcore-daemon-v2/tree/master/4-Call-OwnApi-Pop) for instructions.
- App registration for the Tile Service
  - With an App role of `AccessTileBag`
- App registration of the Word Game app
  - With a client secret to identify the app
  - Grant the API Permission of `AccessTileBag` for the Tile Service API

## TileService (Server app)
TileService is a CoreWCF Server app. It supports one API defined in *ITileService.cs* for `DrawTiles`. 

The app uses the Azure AD integration with ASP.NET Core to enable and perform JWT Authentication. This is done through:
- Adding Nuget references for `Microsoft.Identity.Web` and `Microsoft.VisualStudio.Azure.Containers.Tools.Targets`

- Including the ASP.NET Core and Azure AD SDK support for JWT with:

``` c#
// ASP.NET Core Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddMicrosoftIdentityWebApi(builder.Configuration);
```

- Adding Authentication and Authorization to the ASP.NET Core pipeline with

``` c#
app.UseAuthentication();
app.UseAuthorization();
```

- Attributing the service call with the claims that need to be present

``` c#
[AuthorizeRole("AccessTileBag")]
public IList<GameTile> DrawTiles(int count)
{
  ...
}
```
- Configuring the parameters for the Azure SDK to validate the JWT in appsettings.json

``` json
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "ClientId": "[Enter the Client Id of the service (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]",
    "Domain": "[Enter the domain of your tenant, e.g. contoso.onmicrosoft.com]",
    "TenantId": "[Enter 'common', or 'organizations' or the Tenant Id (Obtained from the Azure portal. Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs), e.g. da41245a5-11b3-996c-00a8-4d99re19f292]"
  }
```

## WordGame (Service Client)

The service client is included in a WebAPI app that exposes a simple HTTP GET endpoint to fetch the tiles.

To make the WCF service calls, the app uses a Service Reference client wrapper, generated from the WSDL from the Tile Service. 

The key parts of the application are:

- At startup, it calls `getAzAdJwtBlob`, which
  - Reads AAD properties from configuration (appsettings.json) and User Secrets
  - Calls AAD to get a JWT for this specific service
- Exposes a WebAPI at `/getTiles` that will make the WCF service call and return the response as JSON
  - Exposes a WebAPI at `/` which redirects to /getTiles with a count parameter
- The `getTiles` function which
  - Uses an `OperationContextScope` to add an http `Authorization` header with the JWT as the value
  - Calls the Tile Service API using the generated wrapper class

The only AAD specific code in this sample is included in the getAzAdJWTBlob function. The same pattern can be followed to retrieve a JWT from other authentication providers, or for performing user authentication, etc.