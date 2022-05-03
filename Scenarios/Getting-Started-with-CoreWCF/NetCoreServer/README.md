# Getting started with Core WCF Server sample

This sample uses the BasicHttpBinding, WsHttpBinding & NetTcpBinding and exposes EndPoints for each binding. It uses ASP.NET Core as the host for the services.

## Bindings

A base address is specified for http & https, which is use as the basis for each of the binding endpoints. This base is also used as the URL for WSDL Discovery.  

## WSDL

The sample will expose a WSDL endpoint at /EchoService for HTTP & HTTPS. A client can be created using the svcutil tool against the WSDL endpoint. See [Walkthrough](https://github.com/CoreWCF/CoreWCF/blob/main/Documentation/Walkthrough.md) for an example of how to use the tool to generate a client.
