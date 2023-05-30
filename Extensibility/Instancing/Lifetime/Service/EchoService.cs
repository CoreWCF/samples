// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.LifeTime
{
    // Service class which implements the service contract interface.
    [CustomLeaseTime(Timeout = 20000)]
    public class EchoService : IEchoService
    {
        public EchoService()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Service instance created.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public string Echo(string value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Echo method invoked with :" + value);
            Console.ForegroundColor = ConsoleColor.Gray;

            return value;
        }
    }
}
