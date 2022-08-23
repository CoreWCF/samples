using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using ServiceReference1;
using System.ServiceModel.Channels;
using System.ServiceModel;

var builder = WebApplication.CreateBuilder(args);

var tileServiceUrl = builder.Configuration["TileServiceUrl"];
var tileService = TileServiceFactory(tileServiceUrl);

//builder.Services.AddSingleton<ITileService>(TileServiceFactory);
var jwt = await getAzAdJwtBlob(builder.Configuration);
var app = builder.Build();

// Use Minimal WebApi Endpoint for the demo functionality
app.Map("/", () => Results.Redirect("/getTiles?Count=5"));
app.MapGet("/getTiles", (int count) => getTiles(tileService, count, jwt));

app.Run();

// Factory for TileService
TileServiceClient TileServiceFactory(string tileServiceUrl)
{
    return new TileServiceClient(TileServiceClient.EndpointConfiguration.BasicHttpBinding_ITileService, tileServiceUrl);
}

//TileServiceClient TileServiceFactory(IServiceProvider provider)
//{
//    var config = provider.GetRequiredService(IConfiguration);
//    var tileServiceUrl = config["TileServiceUrl"];
//    return new TileServiceClient(TileServiceClient.EndpointConfiguration.BasicHttpBinding_ITileService, tileServiceUrl);
//}

async Task<IList<GameTile>> getTiles(TileServiceClient tileService, int count, string jwt)
{
        // Create a scope for this call
        using (new OperationContextScope((tileService).InnerChannel))
        {
            // Add an http Authorization header with the JWT
            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
            requestMessage.Headers["Authorization"] = $"Bearer {jwt}";
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
            return await tileService.DrawTilesAsync(count);
        }
    
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

