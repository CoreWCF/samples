// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Timers;
using Timer = System.Timers.Timer;

namespace CoreWcf.Samples.LifeTime
{
    interface ICustomLease
    {
        bool IsIdle { get; }
        Action<InstanceContext> Callback { get; set; }
    }

    /// <summary>
    /// This class contains the implementation of an extension to InstanceContext. 
    /// This enables extended lifetime for the InstanceContext.
    /// </summary>
    class CustomLeaseExtension : IExtension<InstanceContext>, ICustomLease
    {
        #region Private Fields

        // Reference to the InstanceContext instance owns this 
        // extension instance.
        private InstanceContext _owner;

        private bool _isIdle;
        private object _thisLock;
        private Timer _idleTimer;
        private double _idleTimeout;
        private Action<InstanceContext> _callback;

        #endregion

        public string InstanceId { get; }

        #region Constructor

        public CustomLeaseExtension(double idleTimeout, string instanceId)
        {
            _owner = null;
            _isIdle = false;
            _thisLock = new object();
            _idleTimer = new Timer();
            _idleTimeout = idleTimeout;
            InstanceId = instanceId;
        }

        #endregion

        #region IExtension<InstanceContext> Members

        /// <summary>
        /// Attaches this extension to current instance of 
        /// InstanceContext. 
        /// </summary>       
        /// <remarks>
        /// This method is called by WCF at the time it attaches this
        /// extension.
        /// </remarks>
        public void Attach(InstanceContext owner)
        {
            _owner = owner;
        }

        public void Detach(InstanceContext owner)
        {
        }

        #endregion

        #region ICustomLease Members

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// InstanceContext is idle or not.
        /// </summary>
        public bool IsIdle
        {
            get
            {
                lock (_thisLock)
                {
                    if (_isIdle)
                    {
                        return true;
                    }
                    else
                    {
                        StartTimer();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the InstanceContextIdleCallback.
        /// </summary>
        public Action<InstanceContext> Callback
        {
            get
            {
                lock (_thisLock)
                {
                    return _callback;
                }
            }
            set
            {
                // Immutable state.
                if (_idleTimer.Enabled)
                {
                    throw new InvalidOperationException("Callback could not be changed when the timer is running.");
                }

                lock (_thisLock)
                {
                    _callback = value;
                }
            }
        }

        #endregion

        #region Helper members

        /// <summary>
        /// Starts the timer.
        /// </summary>
        void StartTimer()
        {
            lock (_thisLock)
            {
                _idleTimer.Interval = _idleTimeout;
                _idleTimer.Elapsed += new ElapsedEventHandler(idleTimer_Elapsed);

                if (!_idleTimer.Enabled)
                {
                    _idleTimer.Start();
                }
            }
        }

        public void StopTimer()
        {
            lock (_thisLock)
            {
                if (_idleTimer.Enabled)
                {
                    _idleTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Timer elapsed event handler.
        /// </summary>        
        void idleTimer_Elapsed(object sender, ElapsedEventArgs args)
        {
            lock (_thisLock)
            {
                StopTimer();
                _isIdle = true;
                Utility.WriteMessageToConsole("Custom lease timeout expired. Notifying WCF.");
                _callback(_owner);
            }
        }

        #endregion

    }
}
