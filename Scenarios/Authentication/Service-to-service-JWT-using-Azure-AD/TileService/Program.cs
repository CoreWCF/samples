using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Web.Services.Description;

var builder = WebApplication.CreateBuilder();

// CoreWCF Services
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

//Register the `TileBag` class with Dependency Injection, based on it being a single instance
builder.Services.AddSingleton<TileBag>();

// ASP.NET Core Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddMicrosoftIdentityWebApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Should be turned off for production, but will show user info as part of logging
    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
}

app.UseAuthentication();
app.UseAuthorization();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<TileBag>();
    serviceBuilder.AddServiceEndpoint<TileBag, ITileService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "https://host/TileService");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.MapGet("/", () => "Error incorrect path: Use /TileService to access the service");

app.Run();
