// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var builder = WebApplication.CreateBuilder();

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, ServiceProviderServiceBehavior>()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

//Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>(serviceOptions => { })
    // Add BasicHttpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(new BasicHttpBinding(), "CalculatorService/basicHttp");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
