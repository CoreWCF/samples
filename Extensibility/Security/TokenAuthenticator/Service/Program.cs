// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder();

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>(serviceOptions => { })
    // Add WSHttpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(ServiceWSHttpBinding(), "CalculatorService");

    Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
    builder.ConfigureServiceHostBase<CalculatorService>(serviceHost);

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();

static Binding ServiceWSHttpBinding()
{
    WSHttpBinding binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
    binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
    return binding;
}

void ChangeHostBehavior(ServiceHostBase host)
{
    var srvCredentials = new ServiceCredentials();
    srvCredentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");
    MyUserNameCredential serviceCredential = new MyUserNameCredential();
    host.Description.Behaviors.Remove((typeof(ServiceCredentials)));
    host.Description.Behaviors.Add(serviceCredential);
}
