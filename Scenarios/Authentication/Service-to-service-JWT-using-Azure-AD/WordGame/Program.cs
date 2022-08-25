using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using ServiceReference1;
using System.ServiceModel.Channels;
using System.ServiceModel;
using CoreWCF.Helpers;

await new Program().Run(args);

partial class Program
{
    private Timer? _jwtRefreshTimer;
    private IConfiguration? _configuration;
    private TileServiceClient? _tileService;

    async Task Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        _configuration = builder.Configuration;
        var app = builder.Build();

        var tileServiceUrl = _configuration["TileServiceUrl"];
        _tileService = new TileServiceClient(TileServiceClient.EndpointConfiguration.BasicHttpBinding_ITileService, tileServiceUrl);

        await SetupJWTRefresh();

        // Use Minimal WebApi Endpoint for the demo functionality
        app.Map("/", () => Results.Redirect("/getTiles?Count=5"));
        app.MapGet("/getTiles", (int count) => getTiles(count));

        app.Run();
    }

    async Task<IList<GameTile>> getTiles(int count)
    {
        return await _tileService.DrawTilesAsync(count);
    }

    // Fetch the jwt, configure the header, and setup a timer to refresh the token
    async Task SetupJWTRefresh()
    {
        // Fetch the jwt from AAD
        var _jwt = await getAzAdJwt(_configuration);

        // Set the WCF Client to include the jwt as the `Authorization` header
        _tileService.AddRequestHeader("Authorization", $"Bearer {_jwt.AccessToken}");

        // JWT's have a timeout. Setup a timer 5 mins earlier to fetch a new token
        // AAD service-to-service jwt don't have a refresh token, so fetch again based on client secret
        var timeout = (_jwt.ExpiresOn - DateTimeOffset.Now).Subtract(TimeSpan.FromMinutes(5));
        _jwtRefreshTimer = new Timer((x) => _ = SetupJWTRefresh(), null, timeout, Timeout.InfiniteTimeSpan);
    }


    // Azure Active Directoy specific code to authenticate a daemon app with a service
    // Based on https://github.com/Azure-Samples/active-directory-dotnetcore-daemon-v2/tree/master/4-Call-OwnApi-Pop
    async Task<AuthenticationResult> getAzAdJwt(IConfiguration config)
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

        return result;
    }
}
