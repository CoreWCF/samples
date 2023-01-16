// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Client;
using ServiceReference1;

// Request an access_token
var services = new ServiceCollection();
services.AddOpenIddict()
    .AddClient(options =>
    {
        options.AllowClientCredentialsFlow();
        options.DisableTokenStorage();
        options.UseSystemNetHttp();
        options.AddRegistration(new OpenIddictClientRegistration
            {
                Issuer = new Uri("https://localhost:7222/", UriKind.Absolute),
                ClientId = "console_app",
                ClientSecret = "secret",
                Scopes = { "api" }
            }
        );
    });
var provider = services.BuildServiceProvider();
var service = provider.GetRequiredService<OpenIddictClientService>();
var (tokenResponse, _) = await service.AuthenticateWithClientCredentialsAsync(new Uri("https://localhost:7222/", UriKind.Absolute), new [] { "api" });

Console.WriteLine($"Retrieved access_token {tokenResponse.AccessToken}");

// Create an authenticated client and call 'Echo' and 'EchoAnonymous'
var channelFactory = new ChannelFactory<ISecuredServiceChannel>(new BasicHttpBinding(BasicHttpSecurityMode.Transport),
    new EndpointAddress("https://localhost:7233/MixedAuthService.svc/authenticated"));
var channel = channelFactory.CreateChannel();

var context = new OperationContext(channel);
using (var _ = new OperationContextScope(context))
{
    var httpRequestProperty = new HttpRequestMessageProperty();
    httpRequestProperty.Headers[HttpRequestHeader.Authorization] = $"Bearer {tokenResponse.AccessToken}";
    context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
    var response = await channel.EchoAsync("Hello world");
    Console.WriteLine(response);
    var response2 = await channel.EchoAnonymousAsync("Hey");
    Console.WriteLine(response2);
}

// Create an anonymous client and call 'EchoAnonymous'
var anonymousChannelFactory = new ChannelFactory<IAnonymousServiceChannel>(new BasicHttpBinding(BasicHttpSecurityMode.Transport),
        new EndpointAddress("https://localhost:7233/MixedAuthService.svc/anonymous"));

var anonymousChannel = anonymousChannelFactory.CreateChannel();

var response3 = await anonymousChannel.EchoAnonymousAsync("Bonjour");
Console.WriteLine(response3);

Console.ReadLine();

