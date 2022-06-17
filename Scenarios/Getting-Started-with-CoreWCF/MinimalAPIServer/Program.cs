using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Channels;
using CoreWCF.Description;
using MinimalAPIServer;
using MyContracts;

// Only used in the case that UseRequestHeadersForMetadataAddressBehavior doesn't apply
const string HOST_IN_WSDL = "localhost";

var builder = WebApplication.CreateBuilder(args);

// Add WSDL support
builder.Services.AddServiceModelServices().AddServiceModelMetadata();
// Use the scheme/host/port used to fetch WSDL as that service endpoint address in generated WSDL 
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
var app = builder.Build();

app.UseServiceModel(builder =>
{
    builder.AddService<EchoService>((serviceOptions) =>
    {
        // Set the default host name:port in generated WSDL and the base path for the address 
        serviceOptions.BaseAddresses.Add(new Uri($"http://{HOST_IN_WSDL}/EchoService"));
        serviceOptions.BaseAddresses.Add(new Uri($"https://{HOST_IN_WSDL}/EchoService"));
    })
    .AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(), "/basicHttp")
    .AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/basicHttp");
});

// Enable WSDL for http & https
var serviceMetadataBehavior = app.Services.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;

app.Run();

