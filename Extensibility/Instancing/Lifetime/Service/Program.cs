// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

const int HttpPort = 5000;
const int NetTcpPort = 8089;

var builder = WebApplication.CreateBuilder();

builder.WebHost
.UseKestrel(options =>
{
    options.ListenAnyIP(HttpPort);
})
.UseNetTcp(NetTcpPort);

//Enable CoreWCF Services, with metadata (WSDL) support
builder.Services.AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>()
    .AddSingleton<EchoService>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    // Add the Calculator Service
    builder.AddService<EchoService>(serviceOptions =>
    {
        serviceOptions.BaseAddresses.Clear();
        // Set the default host name:port in generated WSDL and the base path for the address 
        serviceOptions.BaseAddresses.Add(new Uri("http://localhost/EchoService"));
        serviceOptions.BaseAddresses.Add(new Uri($"net.tcp://localhost:{NetTcpPort}/EchoService"));
    })
    // Add NetTcpBinding endpoint
    .AddServiceEndpoint<EchoService, IEchoService>(new NetTcpBinding(SecurityMode.None), "netTcp");

    // Configure WSDL to be available
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

PrintLegend();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Echo service started.");
Console.ForegroundColor = ConsoleColor.Gray;

app.Run();


static void PrintLegend()
{
    Console.WriteLine("=================================");

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Green: Messages from the service host.");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("Blue: Messages from the service instance.");

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Red: Messages from custom lease behavior.");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("=================================");
    Console.WriteLine("");
}
