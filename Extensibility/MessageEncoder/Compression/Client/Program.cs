// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//The service contract is defined using Connected Service "WCF Web Service", generated from the service by the dotnet svcutil tool.

BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
HttpTransportBindingElement httpTransportBindingElement = basicHttpBinding.CreateBindingElements().Find<HttpTransportBindingElement>();
MessageEncodingBindingElement encodingBindingElement = new GZipMessageEncodingBindingElement();
CustomBinding binding = new CustomBinding(new BindingElement[]
{
                encodingBindingElement,
                httpTransportBindingElement
});

var endpointAddress = new EndpointAddress("http://localhost:5000/gzipMessageEncoding");

// Create a client with given client endpoint configuration
EchoServiceClient client = new EchoServiceClient(binding, endpointAddress);

Console.WriteLine("Calling Echo(string):");
Console.WriteLine("Server responds: {0}", client.Echo("Simple hello"));

Console.WriteLine();
Console.WriteLine("Calling BigEcho(string[]):");
string[] messages = new string[64];
for (int i = 0; i < 64; i++)
{
    messages[i] = "Hello " + i;
}

Console.WriteLine("Server responds: {0}", client.BigEcho(messages));

//Closing the client gracefully closes the connection and cleans up resources
client.Close();

Console.WriteLine();
Console.WriteLine("Press <ENTER> to terminate client.");
Console.ReadLine();
