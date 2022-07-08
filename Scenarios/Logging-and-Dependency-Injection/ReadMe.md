## Logging Sample

This sample shows how to use the [`Microsoft.Extensions.Logging`](https://docs.microsoft.com/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0) functionality from a CoreWCF service. It relies on using [Dependency Injection](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0) \(DI\) to create the instance of the `Service` class, passing in the logger to the constructor.

### Simple Dependency Injection

The `Service` class is registered as a singleton with DI as part of the process initialization. CoreWCF will look to see if the service class is registered with DI when it needs an instance. If none is registered, it will construct an instance of the class using reflection (provided there is a default constructor).

This same DI mechanism can be used to pass any necessary utility classes into the service by adding them to the service constructor. 

The logger is created for the `LoggingSample.Service` type, and is configured in [appsettings.json](LoggingSampple/appsettings.json) to log entries for Information level and higher. 

### Scoped Dependency Injection

The `Service2` class demonstrates how to inject a scoped dependency into a service method. The methods that need the dependency include additional parameters for those dependencies, which are marked with the `[Injected]` attribute. CoreWCF will use code generation to create methods that will fulfill the service contract and supply the dependencies to the user method.

For this to work, the following conditions apply:
- The service class needs to be declared `partial` so that it can be extended by code generation
- The service class needs to be registered with DI

For example, the app code:
``` C#
public CompositeType GetDataUsingDataContract(CompositeType composite, [Injected] ILogger<Service2> localLogger) 
{
    ...
}
```

is supplemented by the following generated code:

``` C#
        public LoggingSample.CompositeType GetDataUsingDataContract(LoggingSample.CompositeType composite)
        {
            var serviceProvider = CoreWCF.OperationContext.Current.InstanceContext.Extensions.Find<IServiceProvider>();
            if (serviceProvider == null) throw new InvalidOperationException("Missing IServiceProvider in InstanceContext extensions");
            if (CoreWCF.OperationContext.Current.InstanceContext.IsSingleton)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var d0 = scope.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger<LoggingSample.CompositeType>>();
                    return GetDataUsingDataContract(composite, d0);
                }
            }
            var e0 = serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger<LoggingSample.CompositeType>>();
            return GetDataUsingDataContract(composite, e0);
        }
```
 
This sample uses DI to inject a logger, but this pattern can be used for other dependencies such as a [`DbContext`](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbcontext), where localized scoping is required. The DI scope is matched to the [`InstanceContextMode` of the `ServiceBehavior` attribute](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.servicebehaviorattribute.instancecontextmode?view=netframework-4.8#system-servicemodel-servicebehaviorattribute-instancecontextmode). If you are using `InstanceContextMode.Single`, then all service calls will share a single DI scope. If InstanceContextMode is either `PerCall` or `PerSession`, then a new scope is created for every call or session. The default is PerSession, which is effectively PerCall whenever you aren't using a sessionful binding. The instancing of the dependency is important if the injected instance isn't thread safe, as is the case with DbContext.

## Logging sample client

A client designed to call the LoggingSample services. The client uses service wrappers created using the "Add Service Reference" feature in Visual Studio. 
