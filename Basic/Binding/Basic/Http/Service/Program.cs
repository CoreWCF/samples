// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoreWcf.Samples.Http
{
    public class Program
    {
        static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .UseStartup<BasicHttpBindingStartup>();
    }
}
