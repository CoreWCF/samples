using System.Diagnostics;
using System.Net;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        // Listen on 8088 for http, and 8443 for https, 8089 for NetTcp.
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options => {
                options.ListenAnyIP(Startup.HTTP_PORT);
                options.ListenAnyIP(Startup.HTTPS_PORT, listenOptions =>
                {
                    listenOptions.UseHttps();
                    if (Debugger.IsAttached)
                    {
                        listenOptions.UseConnectionLogging();
                    }
                });
            })
            .ConfigureServices(options =>
            {
                options.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
            })
            .UseNetTcp(Startup.NETTCP_PORT)
            .UseStartup<Startup>();
    }
}
