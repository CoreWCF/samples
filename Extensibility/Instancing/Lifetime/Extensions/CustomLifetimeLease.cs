// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.Dispatcher;

namespace CoreWcf.Samples.LifeTime
{
    public static class CustomHeader
    {
        public static readonly string HeaderName = "InstanceId";
        public static readonly string HeaderNamespace = "http://CoreWcf.Samples.LifeTime/Lifetime";
    }

    /// <summary>
    /// This class contains the implementation for 
    /// custom lifetime lease. It implements 
    /// IShareableInstanceContextLifetime in order to be able 
    /// to attach to the service model layer.
    /// </summary>
    class CustomLifetimeLease : IInstanceContextProvider
    {
        #region Private Fields

        private double _timeout;
        private bool _isIdle;
        private Dictionary<string, InstanceContext> _instanceContextCache;

        // Lock must be acquired on this before
        // accessing the isIdle member.
        // ===============
        // VERY IMPORTANT: 
        // ===============
        // This is a simple approach to make it work  
        // with current API.
        // This approach is highly not acceptable as 
        // it will result in a considerable perf hit or 
        // even cause dead locks depending on how 
        // service model handles threads. 
        private object _thisLock;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of CustomLifetimeLease class.
        /// </summary>
        public CustomLifetimeLease(double timeout)
        {
            _timeout = timeout;
            _thisLock = new object();
            _instanceContextCache = new Dictionary<string, InstanceContext>();
        }

        #endregion

        #region IInstanceContextProvider Members

        public bool IsIdle(InstanceContext instanceContext)
        {
            lock (_thisLock)
            {
                if (_isIdle)
                {
                    Utility.WriteMessageToConsole("WCF is checking whether the instance is Idle. Reply with 'True'");
                }
                else
                {
                    Utility.WriteMessageToConsole("WCF is checking whether the instance is Idle. Reply with 'False'");
                }

                bool idleCopy = _isIdle;
                _isIdle = false;
                return idleCopy;
            }
        }

        public void NotifyIdle(Action<InstanceContext> callback,
            InstanceContext instanceContext)
        {
            lock (_thisLock)
            {
                ICustomLease customLease =
                    instanceContext.Extensions.Find<ICustomLease>();

                customLease.Callback = callback;
                _isIdle = customLease.IsIdle;
                if (_isIdle)
                {
                    callback(instanceContext);
                }
            }
        }

        /// <summary>
        /// This implements a PerCall InstanceContextMode behavior. If a cached InstanceContext is not found
        /// then WCF will create a new one.
        /// </summary>     
        public InstanceContext GetExistingInstanceContext(Message message, IContextChannel channel)
        {
            //Per Session behavior
            //To implement a PerSession behavior (If underlyin binding supports it) where in all 
            //methods from one ChannelFactory will be serviced by the same InstanceContext

            //Check if the incoming request has the InstanceContext id it wants to connect with.
            if (message.Headers.FindHeader(CustomHeader.HeaderName, CustomHeader.HeaderNamespace) != -1)
            {
                string sharingId = message.Headers.GetHeader<string>(CustomHeader.HeaderName, CustomHeader.HeaderNamespace);
                if (sharingId != null && _instanceContextCache.ContainsKey(sharingId))
                {
                    Utility.WriteMessageToConsole(string.Format("WCF is checking to see whether an InstanceContext with Id '{0}' has been cached.", sharingId));
                    //Retrieve the InstanceContext from the map
                    InstanceContext context = _instanceContextCache[sharingId];
                    if (context != null)
                    {
                        //Before returning, stop the timer on this InstanceContext
                        CustomLeaseExtension extension = context.Extensions.Find<CustomLeaseExtension>();
                        Utility.WriteMessageToConsole(string.Format("WCF is allocating InstanceContext with Id '{0}' to process new Message. Stopping the InstanceContext idle timer.", sharingId));
                        extension.StopTimer();

                        Utility.WriteMessageToConsole("WCF found cached InstanceContext. Returning this InstanceContext");
                        return _instanceContextCache[sharingId];
                    }
                }
            }

            //No existing InstanceContext was found so return null and WCF will create a new one.
            return null;
        }

        public void InitializeInstanceContext(InstanceContext instanceContext, Message message, IContextChannel channel)
        {
            //Look if the Client has given us a unique ID to add to this InstanceContext
            int headerIndex = message.Headers.FindHeader(CustomHeader.HeaderName, CustomHeader.HeaderNamespace);
            string headerId = null;
            if (headerIndex != -1)
            {
                headerId = message.Headers.GetHeader<string>(headerIndex);
            }

            if (headerId == null)
            {
                //If no header was sent by the Client, then create a new one and assign it to this InstanceContext.
                headerId = Guid.NewGuid().ToString();
                Utility.WriteMessageToConsole("No header was found in the incoming message. WCF creating a new Id to associate with this InstanceContext.");
            }

            Utility.WriteMessageToConsole(string.Format("WCF created new InstanceContext with unique Id '{0}'. Adding this to cache.", headerId));

            //Add this to the Cache
            _instanceContextCache[headerId] = instanceContext;

            //Register the Closing event of this InstancContext so it can be removed from the collection
            instanceContext.Closing += RemoveInstanceContext;

            IExtension<InstanceContext> customLeaseExtension =
                new CustomLeaseExtension(_timeout, headerId);
            instanceContext.Extensions.Add(customLeaseExtension);
        }

        public void RemoveInstanceContext(object o, EventArgs args)
        {
            InstanceContext context = o as InstanceContext;
            CustomLeaseExtension extension = context.Extensions.Find<CustomLeaseExtension>();
            string id = (extension != null) ? extension.InstanceId : null;
            if (_instanceContextCache[id] != null)
            {
                Utility.WriteMessageToConsole(string.Format("WCF closed InstanceContext with Id '{0}'. Removing this from cache.", id));
                _instanceContextCache.Remove(id);
            }
        }

        #endregion
    }
}
