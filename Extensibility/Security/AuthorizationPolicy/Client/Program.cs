// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;

//The service contract is defined using Connected Service "WCF Web Service", generated from the service by the dotnet svcutil tool.

// Create a client with Username endpoint configuration
WSHttpBinding binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
var endpointAddress = new EndpointAddress("https://localhost:5001/CalculatorService/Username");

CalculatorServiceClient client = new CalculatorServiceClient(binding, endpointAddress);
client.ClientCredentials.UserName.UserName = "test1";
client.ClientCredentials.UserName.Password = "1test";

CallServiceOperations(client);

// Create a client with Certificate endpoint configuration
binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
endpointAddress = new EndpointAddress("https://localhost:5001/CalculatorService/Certificate");

client = new CalculatorServiceClient(binding, endpointAddress);
client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "test1");
client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;

CallServiceOperations(client);

Console.WriteLine();
Console.WriteLine("Press <ENTER> to terminate client.");
Console.ReadLine();


void CallServiceOperations(CalculatorServiceClient client)
{
    try
    {
        // Call the Add service operation.
        double value1 = 100.00D;
        double value2 = 15.99D;
        double result = client.Add(value1, value2);
        Console.WriteLine("Add({0},{1}) = {2}", value1, value2, result);

        // Call the Subtract service operation.
        value1 = 145.00D;
        value2 = 76.54D;
        result = client.Subtract(value1, value2);
        Console.WriteLine("Subtract({0},{1}) = {2}", value1, value2, result);

        // Call the Multiply service operation.
        value1 = 9.00D;
        value2 = 81.25D;
        result = client.Multiply(value1, value2);
        Console.WriteLine("Multiply({0},{1}) = {2}", value1, value2, result);

        // Call the Divide service operation.
        value1 = 22.00D;
        value2 = 7.00D;
        result = client.Divide(value1, value2);
        Console.WriteLine("Divide({0},{1}) = {2}", value1, value2, result);
    }
    catch (Exception e)
    {
        Console.WriteLine("Call failed : {0}", e.Message);
    }
    //Closing the client gracefully closes the connection and cleans up resources
    client.CloseAsync();
}
