// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var builder = WebApplication.CreateBuilder();

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<EchoService>(serviceOptions => { });

    BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
    HttpTransportBindingElement httpTransportBindingElement = basicHttpBinding.CreateBindingElements().Find<HttpTransportBindingElement>();
    MessageEncodingBindingElement encodingBindingElement = new GZipMessageEncodingBindingElement();
    CustomBinding binding = new CustomBinding(new BindingElement[]
    {
                encodingBindingElement,
                httpTransportBindingElement
    });

    builder.AddServiceEndpoint<EchoService, IEchoService>(binding, "gzipMessageEncoding");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
