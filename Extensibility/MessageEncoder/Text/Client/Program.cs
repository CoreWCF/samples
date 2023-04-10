// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//The service contract is defined using Connected Service "WCF Web Service", generated from the service by the dotnet svcutil tool.

BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
HttpTransportBindingElement httpTransportBindingElement = basicHttpBinding.CreateBindingElements().Find<HttpTransportBindingElement>();
MessageEncodingBindingElement encodingBindingElement = new CustomTextMessageBindingElement("UTF-8", "application/soap+xml", MessageVersion.Soap12WSAddressing10);
httpTransportBindingElement.TransferMode = TransferMode.Streamed;
CustomBinding binding = new CustomBinding(new BindingElement[]
{
                encodingBindingElement,
                httpTransportBindingElement
});

var endpointAddress = new EndpointAddress("https://localhost:5001/CalculatorService");

// Create a client with given client endpoint configuration
CalculatorServiceClient client = new CalculatorServiceClient(binding, endpointAddress);

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

//Closing the client gracefully closes the connection and cleans up resources
client.CloseAsync();

Console.WriteLine();
Console.WriteLine("Press <ENTER> to terminate client.");
Console.ReadLine();
