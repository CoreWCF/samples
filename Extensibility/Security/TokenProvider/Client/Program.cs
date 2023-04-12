// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ServiceModel.Description;
using System.ServiceModel.Security;

//The service contract is defined using Connected Service "WCF Web Service", generated from the service by the dotnet svcutil tool.

CalculatorServiceClient client = new CalculatorServiceClient();
// set new credentials
client.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
client.ChannelFactory.Endpoint.EndpointBehaviors.Add(new MyUserNameClientCredentials());
/*
Setting the CertificateValidationMode to PeerOrChainTrust means that if the certificate 
is in the Trusted People store, then it will be trusted without performing a
validation of the certificate's issuer chain. This setting is used here for convenience so that the 
sample can be run without having to have certificates issued by a certificate authority (CA).
This setting is less secure than the default, ChainTrust. The security implications of this 
setting should be carefully considered before using PeerOrChainTrust in production code. 
*/
client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

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
await client.CloseAsync();

Console.WriteLine();
Console.WriteLine("Press <ENTER> to terminate client.");
Console.ReadLine();

