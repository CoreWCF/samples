// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RabbitMQ_TLS;

public class EchoService : IEchoService
{
    public string Echo(string text)
    {
        System.Console.WriteLine($"Received {text} from client!");
        return text;
    }

    public string ComplexEcho(EchoMessage text)
    {
        System.Console.WriteLine($"Received {text.Text} from client!");
        return text.Text;
    }

    public string FailEcho(string text)
        => throw new FaultException<EchoFault>(new EchoFault() { Text = "WCF Fault OK" }, new FaultReason("FailReason"));
}
