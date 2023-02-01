// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CoreWcf.Samples.CompressionMessageEncoder
{
    // Service class which implements the service contract interface.
    public class EchoService : IEchoService
    {
        public string Echo(string input)
        {
            Console.WriteLine("\n\tServer Echo(string input) called:", input);
            Console.WriteLine("\tClient message:\t{0}\n", input);
            return input + " " + input;
        }

        public string[] BigEcho(string[] input)
        {
            Console.WriteLine("\n\tServer BigEcho(string[] input) called:", input);
            Console.WriteLine("\t{0} client messages", input.Length);
            return input;
        }
    }
}
