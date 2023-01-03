using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using IdentityModel.Client;
using ServiceReference1;

using HttpClient httpClient = new HttpClient();
var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync("https://demo.duendesoftware.com/.well-known/openid-configuration");
var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discoveryDocumentResponse.TokenEndpoint,
    ClientId = "m2m",
    ClientSecret = "secret",
    Scope = "api"
});

var channelFactory = new ChannelFactory<ISecuredServiceChannel>(new BasicHttpBinding(BasicHttpSecurityMode.Transport),
    new EndpointAddress("https://localhost:7173/Service.svc"));
var channel = channelFactory.CreateChannel();

var httpRequestProperty = new HttpRequestMessageProperty();
httpRequestProperty.Headers[HttpRequestHeader.Authorization] = $"Bearer {tokenResponse.AccessToken}";
var context = new OperationContext(channel);
using var operationContextScope = new OperationContextScope(context);
context.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
var response = await channel.EchoAsync("Hello world");

Console.WriteLine(response);
Console.ReadKey();

