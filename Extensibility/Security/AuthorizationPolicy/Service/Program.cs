// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography.X509Certificates;
using CoreWCF.IdentityModel.Policy;

var builder = WebApplication.CreateBuilder();

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<ServiceAuthorizationManager, CustomServiceAuthorizationManager>()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<CalculatorService>(serviceOptions =>
    {
        serviceOptions.BaseAddresses.Clear();
        // Set the default host name:port in generated WSDL and the base path for the address 
        serviceOptions.BaseAddresses.Add(new Uri("https://localhost/CalculatorService"));
    })
    // Add WSHttpBinding endpoint
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(ServiceWSHttpBinding(MessageCredentialType.UserName), "Username")
    .AddServiceEndpoint<CalculatorService, ICalculatorService>(ServiceWSHttpBinding(MessageCredentialType.Certificate), "Certificate");

    Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
    builder.ConfigureServiceHostBase<CalculatorService>(serviceHost);

    ServiceAuthorizationBehavior authBehavior = app.Services.GetRequiredService<ServiceAuthorizationBehavior>();
    var authPolicies = new List<IAuthorizationPolicy> { new CustomAuthorizationPolicy() };
    var externalAuthPolicies = new System.Collections.ObjectModel.ReadOnlyCollection<IAuthorizationPolicy>(authPolicies);
    authBehavior.ExternalAuthorizationPolicies = externalAuthPolicies;

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();

static Binding ServiceWSHttpBinding(MessageCredentialType clientCredentialType)
{
    WSHttpBinding binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
    binding.Security.Message.ClientCredentialType = clientCredentialType;
    return binding;
}

void ChangeHostBehavior(ServiceHostBase host)
{
    var srvCredentials = new ServiceCredentials();
    srvCredentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");
    srvCredentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
    srvCredentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
    srvCredentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNameValidator();
    host.Description.Behaviors.Add(srvCredentials);
}
