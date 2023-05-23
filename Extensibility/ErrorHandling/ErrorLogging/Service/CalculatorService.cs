// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.Channels;
using CoreWCF.Dispatcher;

namespace CoreWcf.Samples.ErrorLogging
{
    // Service class which implements the service contract interface.
    //The ErrorBehaviorAttribute is used to install the custom error handler.
    [ErrorBehavior(typeof(CalculatorErrorHandler))]
    public class CalculatorService : ICalculatorService
    {
        public int Add(int n1, int n2)
        {
            return n1 + n2;
        }

        public int Subtract(int n1, int n2)
        {
            return n1 - n2;
        }

        public int Multiply(int n1, int n2)
        {
            return n1 * n2;
        }

        public int Divide(int n1, int n2)
        {
            try
            {
                return n1 / n2;
            }
            catch (DivideByZeroException)
            {
                throw new FaultException("Invalid Argument: The second argument must not be zero.");
            }
        }

        public int Factorial(int n)
        {
            if (n < 1)
                throw new FaultException("Invalid Argument: The argument must be greater than zero.");

            int factorial = 1;
            for (int i = 1; i <= n; i++)
            {
                factorial = factorial * i;
            }
            return factorial;
        }
    }

    public class CalculatorErrorHandler : IErrorHandler
    {
        private ILogger _logger;
        // Provide a fault. The Message fault parameter can be replaced, or set to
        // null to suppress reporting a fault.
        public CalculatorErrorHandler(ILogger<CalculatorErrorHandler> logger)
        {
            _logger = logger;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        // HandleError. Log an error, then allow the error to be handled as usual.
        // Return true if the error is considered as already handled
        public bool HandleError(Exception error)
        {
            _logger.LogInformation("Exception: " + error.GetType().Name + " - " + error.Message);

            return true;
        }
    }

    // This attribute can be used to install a custom error handler for a service
    public sealed class ErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        public ErrorBehaviorAttribute(Type errorHandlerType)
        {
            ErrorHandlerType = errorHandlerType;
        }

        public Type ErrorHandlerType { get; }

        void IServiceBehavior.Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;
            var serviceProvider = description.Behaviors.Find<ServiceProviderServiceBehavior>().ServiceProvider;

            try
            {
                errorHandler = (IErrorHandler)ActivatorUtilities.CreateInstance(serviceProvider, ErrorHandlerType);
            }
            catch (MissingMethodException e)
            {
                throw new ArgumentException("The errorHandlerType specified in the ErrorBehaviorAttribute constructor must have a public empty constructor.", e);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("The errorHandlerType specified in the ErrorBehaviorAttribute constructor must implement CoreWCF.Dispatcher.IErrorHandler.", e);
            }

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }
    }
}
