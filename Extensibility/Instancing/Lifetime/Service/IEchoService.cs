// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.LifeTime
{
    // Define a service contract.
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string value);
    }
}
