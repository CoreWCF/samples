// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

const int HttpPort = 5000;
const int NetTcpPort = 8089;
var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                         ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateBuilder(options);

// Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddHostedService<WindowsServiceWorker>()
        .AddServiceModelServices()
        .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>()
        .AddServiceModelMetadata();

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(HttpPort);
})
.UseNetTcp(NetTcpPort);

builder.Host.UseWindowsService(options =>
{
    // Set service name (Optional)
    options.ServiceName = "CoreWCF Windows Service";
});

var app = builder.Build();

// Configure the bindings and endpoints
app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>(serviceOptions =>
    {
        serviceOptions.BaseAddresses.Clear();
        // Set the default host name:port in generated WSDL and the base path for the address 
        serviceOptions.BaseAddresses.Add(new Uri("http://localhost/CalculatorService"));
        serviceOptions.BaseAddresses.Add(new Uri($"net.tcp://localhost:{NetTcpPort}/CalculatorService"));
    })
    // Add BasicHttpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(new BasicHttpBinding(), "basicHttp")
    // Add NetTcpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(new NetTcpBinding(), "netTcp");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
