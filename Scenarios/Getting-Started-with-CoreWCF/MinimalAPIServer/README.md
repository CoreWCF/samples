# Minimal API CoreWCF Sample

This sample shows how to use CoreWCF with .NET 6 using the [Minimal API Syntax](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0). 

This sample uses the BasicHttpBinding and exposes endpoints at `http://*:8088/EchoService/basichttp` and `https://*:8443/EchoService/basichttp`.
The URL comes from a combination of:
* The host:port specified by the URLs property in [appsettings.json](appsettings.json) or on the cmd line
* The base path from the BaseAddress property specified in code
* Combined with the path from the service EndPoint registration.

## Bindings

The bindings can be changed via code, for example to use WSHttpBinding, the Endpoint can be changed to (or additionally exposed with):

``` C#
    .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.Transport), "/EchoService/wshttp");
```

## WSDL

The sample will expose a WSDL endpoint at /EchoService?wsdl for HTTP & HTTPS, eg
```
curl -k https://localhost:8443/EchoService?wsdl
```
The WSDL will use the address the WSDL was requested from for that endpoint, due to the line `builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();`
For other endpoints it will use the URL specified in BaseAddress.
