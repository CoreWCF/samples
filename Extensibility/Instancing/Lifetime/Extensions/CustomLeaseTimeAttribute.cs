// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CoreWCF.Dispatcher;

namespace CoreWcf.Samples.LifeTime
{
    /// <summary>
    /// This class contains the implementation for the attribute
    /// used to add custom lease time to the service instance.
    /// </summary>
    public sealed class CustomLeaseTimeAttribute : Attribute, IServiceBehavior
    {
        #region Private Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the custom lease time.
        /// </summary>
        public double Timeout { get; set; }

        #endregion

        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {

        }

        /// <summary>
        /// Applies the custom lease time behavior.
        /// </summary>        
        /// <remarks>
        /// This method is invoked by WCF runtime.
        /// </remarks>
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            CustomLifetimeLease customLease = new CustomLifetimeLease(Timeout);

            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;

                if (cd != null)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceContextProvider = customLease;
                    }
                }
            }
        }

        public void Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {

        }

        #endregion
    }
}
