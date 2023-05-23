// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.ErrorLogging
{
    // Define a service contract
    [ServiceContract(Namespace = "http://CoreWcf.Samples.ErrorLogging")]
    public interface ICalculatorService
    {
        [OperationContract]
        int Add(int n1, int n2);

        [OperationContract]
        int Subtract(int n1, int n2);

        [OperationContract]
        int Multiply(int n1, int n2);

        [OperationContract]
        int Divide(int n1, int n2);

        [OperationContract]
        int Factorial(int n);
    }
}
