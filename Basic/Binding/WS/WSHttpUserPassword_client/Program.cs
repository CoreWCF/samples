// See https://aka.ms/new-console-template for more information
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("WsHttpUserPassword Client");

var client = new ServiceReference1.EchoServiceClient(ServiceReference1.EchoServiceClient.EndpointConfiguration.AuthenticatedEP);
var up = client.ClientCredentials.UserName;
up.UserName = "ImValid";
up.Password = "passwordIsValid";

client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
client.ClientCredentials.ServiceCertificate.Authentication.CustomCertificateValidator = new MyCertValidator();

await client.OpenAsync();
var result = await client.EchoAsync("This should work");
await client.CloseAsync();


class MyCertValidator : X509CertificateValidator
{
    public override void Validate(X509Certificate2 certificate)
    {
        // Really bad, does nothing
    }
}

