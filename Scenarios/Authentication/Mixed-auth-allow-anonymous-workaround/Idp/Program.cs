// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenIddict()
    // The OpenIddict configuration below provides a minimal OpenId Provider
    // which only supports the client_credentials flow for demo purpose.
    .AddServer(options =>
    {
        options.AddDevelopmentEncryptionCertificate();
        options.AddDevelopmentSigningCertificate();
        options.AllowClientCredentialsFlow();
        options.SetTokenEndpointUris("token");
        options.EnableDegradedMode();
        options.UseAspNetCore();
        options.RegisterScopes("api");
        options.DisableAccessTokenEncryption();
        options.AddEventHandler<OpenIddictServerEvents.ValidateTokenRequestContext>(options =>
            options.UseInlineHandler(context =>
            {
                if (!context.Request.IsClientCredentialsGrantType())
                {
                    context.Reject(error: OpenIddictConstants.Errors.InvalidGrant);
                    return default;
                }

                if (!string.Equals(context.ClientId, "console_app", StringComparison.Ordinal))
                {
                    context.Reject(error: OpenIddictConstants.Errors.InvalidClient);
                    return default;
                }

                // In real world the client_secret would be a time constant derivation of the client's secret
                // to mitigate statistical attacks.
                // Here we used the "secret" value for readability.
                if (!string.Equals(context.ClientSecret, "secret", StringComparison.Ordinal))
                {
                    context.Reject(error: OpenIddictConstants.Errors.InvalidClient);
                    return default;
                }

                if (string.IsNullOrEmpty(context.Request.Scope))
                {
                    context.Reject(error: OpenIddictConstants.Errors.InvalidScope);
                }

                return default;
            }));
        options.AddEventHandler<OpenIddictServerEvents.HandleTokenRequestContext>(options =>
        {
            options.UseInlineHandler(context =>
            {
                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: OpenIddictConstants.Claims.Name,
                    roleType: OpenIddictConstants.Claims.Role);

                identity.SetScopes("api");
                identity.SetAudiences("api");
                identity.SetClaim(OpenIddictConstants.Claims.Subject, context.ClientId);

                context.Principal = new ClaimsPrincipal(identity);
                return default;
            });
        });
    });

var app = builder.Build();

app.MapGet("/", () => Results.Ok());

app.Run();
