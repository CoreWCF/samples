// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var builder = WebApplication.CreateBuilder();
builder.WebHost.UseKestrel(options =>
{
    options.AllowSynchronousIO = true;
});

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>(serviceOptions => { });

    BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
    HttpTransportBindingElement httpTransportBindingElement = basicHttpBinding.CreateBindingElements().Find<HttpTransportBindingElement>();
    MessageEncodingBindingElement encodingBindingElement = new CustomTextMessageBindingElement("UTF-8", "application/soap+xml", MessageVersion.Soap12WSAddressing10);
    httpTransportBindingElement.TransferMode = TransferMode.Streamed;
    CustomBinding binding = new CustomBinding(new BindingElement[]
    {
                encodingBindingElement,
                httpTransportBindingElement
    });

    builder.AddServiceEndpoint<CalculatorService, ICalculatorService>(binding, "CalculatorService");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
