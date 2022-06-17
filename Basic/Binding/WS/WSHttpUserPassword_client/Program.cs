// See https://aka.ms/new-console-template for more information
using System.ServiceModel;
using System.ServiceModel.Security;

Console.WriteLine("WSHttpUserPassword Client");

// Currently dotnet-svcutil produces the wrong binding for a WSHttpBinding endpoint using TransportWithMessageCredential
// See https://github.com/dotnet/wcf/issues/4828

var wsHttpBindingWithCredential = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
wsHttpBindingWithCredential.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
var client = new ServiceReference1.EchoServiceClient(wsHttpBindingWithCredential,
        new EndpointAddress("https://localhost:8443/EchoService/wsHttpUserPassword"));

var up = client.ClientCredentials.UserName;
up.UserName = "ImValid";
up.Password = "passwordIsValid";

var cert = new X509ServiceCertificateAuthentication();
cert.CertificateValidationMode = X509CertificateValidationMode.None;

client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = cert;


await client.OpenAsync();
var result = await client.EchoAsync("An authenticated request");
await client.CloseAsync();

Console.WriteLine(result);


