// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

const int NetTcpPort = 8089;

var builder = WebApplication.CreateBuilder();

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>()
    .AddSingleton<CalculatorService>();
builder.WebHost.UseNetTcp(NetTcpPort);

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>()
    // Add NetTcpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(new NetTcpBinding(), "CalculatorService/netTcp");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
    serviceMetadataBehavior.HttpGetUrl = new Uri($"http://localhost/CoreWcfSamples/netTcp/CalculatorService");
});

app.Run();
