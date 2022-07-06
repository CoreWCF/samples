## Logging Sample

This sample shows how to use the [`Microsoft.Extensions.Logging`](https://docs.microsoft.com/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0) functionality from a CoreWCF service. It relies on using [Dependency Injection](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0) \(DI\) to create the instance of the `Service` class, passing in the logger to the constructor.

The `Service` class is registered as a singleton with DI as part of the process initialization. CoreWCF will look to see if the service class is registered with DI when it needs an instance. If none is registered, it will construct an instance of the class using reflection.

This same DI mechanism can be used to pass any necessary utility classes into the service by adding them to the service constructor. 

The logger is created for the `LoggingSample.Service` type, and is configured in [appsettings.json](LoggingSampple/appsettings.json) to log entries for Information level and higher. 

## Logging sample client

A client designed to call the LoggingSample service. The client uses a service wrapper created using the Add Service Reference feature in Visual Studio. 
