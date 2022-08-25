using CoreWCF.Helpers;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using ServiceReference1;

var builder = WebApplication.CreateBuilder(args);

var tileServiceUrl = builder.Configuration["TileServiceUrl"];

//builder.Services.AddSingleton<ITileService>(TileServiceFactory);
var jwt = await getAzAdJwtBlob(builder.Configuration);
var tileService = TileServiceFactory(tileServiceUrl, jwt);
var app = builder.Build();

// Use Minimal WebApi Endpoint for the demo functionality
app.Map("/", () => Results.Redirect("/getTiles?Count=5"));
app.MapGet("/getTiles", (int count) => getTiles(tileService, count));

app.Run();

// Factory for TileService
TileServiceClient TileServiceFactory(string tileServiceUrl, string jwt)
{
    var client = new TileServiceClient(TileServiceClient.EndpointConfiguration.BasicHttpBinding_ITileService, tileServiceUrl);

    // Use a helper from HttpRequestBehaviors.cs to add a client behavior that adds http headers
    // to outbound requests
    client.AddRequestHeader("Authorization", $"Bearer {jwt}");
    return client;
}

async Task<IList<GameTile>> getTiles(TileServiceClient tileService, int count)
{
    return await tileService.DrawTilesAsync(count);
}

// Azure Active Directoy specific code to authenticate a daemon app with a service
// Based on https://github.com/Azure-Samples/active-directory-dotnetcore-daemon-v2/tree/master/4-Call-OwnApi-Pop
async Task<string> getAzAdJwtBlob(ConfigurationManager config)
{
    // Fetch the auth details from configuration or the environment for secrets
    var adTenant = config["AzureAdTenant"];
    var clientId = config["ClientId"];
    var clientSecret = config["ClientSecret"];
    var tileServiceScope = config["TileServiceScope"];

    var authority = new Uri($"https://login.microsoftonline.com/{adTenant}");
    var scopes = new string[] { tileServiceScope };

    var app = ConfidentialClientApplicationBuilder.Create(clientId)
                  .WithClientSecret(clientSecret)
                  .WithAuthority(authority)
                  .Build();

    app.AddInMemoryTokenCache();

    AuthenticationResult result;
    try
    {
        result = await app.AcquireTokenForClient(scopes)
            .ExecuteAsync();
    }
    catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
    {
        // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
        // Mitigation: change the scope to be as expected

        // Rethrow so we can see it in the application logs
        throw ex;
    }

    return result.AccessToken;
}

