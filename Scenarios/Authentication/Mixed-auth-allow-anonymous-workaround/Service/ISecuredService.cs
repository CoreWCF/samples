// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Service;

[ServiceContract]
public interface ISecuredService : IAnonymousService
{
    [OperationContract]
    string Echo(string value);
}
