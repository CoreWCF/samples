# Minimal API CoreWCF Sample

This sample shows how to use CoreWCF with .NET 6 using the [Minimal API Syntax](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0). 

This sample uses the BasicHttpBinding and exposes EndPoints at `http://localhost:5000/EchoService/basichttp` and `https://localhost:5001/EchoService/basichttp`. The URL comes from the BaseAddress property specified in code combined with the path from the service EndPoint registration.

## Bindings

The bindings can be changed via code, for example to use WSHttpBinding, the Endpoint can be changed to (or additionally exposed with):

``` C#
    .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.Transport), "/EchoService/wshttp");
```

See the [Basic / Bindings](Basic/Bindings) section for more examples. 

## WSDL

The sample will expose a WSDL endpoint at /EchoService for HTTP & HTTPS.


