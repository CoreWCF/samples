// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ServiceModel.Channels;

NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
var endpointAddress = new EndpointAddress("net.tcp://localhost:8089/EchoService/netTcp");

Console.WriteLine("Press <enter> to open a channel and send a request.");

Console.ReadLine();

MessageHeader shareableInstanceContextHeader = MessageHeader.CreateHeader(
        CustomHeader.HeaderName,
        CustomHeader.HeaderNamespace,
        Guid.NewGuid().ToString());

ChannelFactory<IEchoService> channelFactory = new ChannelFactory<IEchoService>(binding, endpointAddress);
IEchoService proxy = channelFactory.CreateChannel();

using (new OperationContextScope((IClientChannel)proxy))
{
    OperationContext.Current.OutgoingMessageHeaders.Add(shareableInstanceContextHeader);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Service returned: " + proxy.Echo("Apple"));
    Console.ForegroundColor = ConsoleColor.Gray;
}

((IChannel)proxy).Close();

Console.WriteLine("Channel No 1 closed.");

Console.WriteLine(
    "Press <ENTER> to send another request from a different channel " +
    "to the same instance. ");

Console.ReadLine();

proxy = channelFactory.CreateChannel();

using (new OperationContextScope((IClientChannel)proxy))
{
    OperationContext.Current.OutgoingMessageHeaders.Add(shareableInstanceContextHeader);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Service returned: " + proxy.Echo("Apple"));
    Console.ForegroundColor = ConsoleColor.Gray;
}

((IChannel)proxy).Close();

Console.WriteLine("Channel No 2 closed.");
Console.WriteLine("Press <ENTER> to complete test.");
Console.ReadLine();


[ServiceContract(SessionMode = SessionMode.Required)]
interface IEchoService
{
    [OperationContract]
    string Echo(string value);
}

public static class CustomHeader
{
    public static readonly string HeaderName = "InstanceId";
    public static readonly string HeaderNamespace = "http://CoreWcf.Samples.LifeTime/Lifetime";
}
