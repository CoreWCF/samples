// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.Audience = "api";
    });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .RequireAssertion(context =>
        {
            string[] scopes = context.User.FindFirst("scope")?.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                ?? Array.Empty<string>();
            return scopes.Any(x => string.Equals(x, "api", StringComparison.Ordinal));
        })
        .Build();
});
builder.Services.AddTransient<SecuredService>();
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<SecuredService>();
    serviceBuilder.AddServiceEndpoint<SecuredService, ISecuredService>(new BasicHttpBinding
    {
        Security = new BasicHttpSecurity
        {
            Mode = BasicHttpSecurityMode.Transport,
            Transport = new HttpTransportSecurity
            {
                ClientCredentialType = HttpClientCredentialType.InheritedFromHost
            }
        }
    }, "/Service.svc");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
