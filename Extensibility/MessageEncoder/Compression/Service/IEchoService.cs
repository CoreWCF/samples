// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.CompressionMessageEncoder
{
    // Define a service contract.
    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string input);

        [OperationContract]
        string[] BigEcho(string[] input);
    }
}
