// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

const int HttpPort = 5000;
const int NetTcpPort = 8089;

var builder = WebApplication.CreateBuilder();

builder.WebHost
.UseKestrel(options =>
{
    options.ListenAnyIP(HttpPort);
})
.UseNetTcp(NetTcpPort);

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>()
    .AddSingleton<CalculatorService>();

var app = builder.Build();

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
    // Add NetTcpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(new NetTcpBinding(), "netTcp");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
