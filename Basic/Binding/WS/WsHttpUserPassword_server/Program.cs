using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreServer
{
    class Program
    {
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;

        static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options =>
            {
                options.AllowSynchronousIO = true;
                options.ListenLocalhost(HTTP_PORT);
                options.ListenLocalhost(HTTPS_PORT, listenOptions =>
                {
                    listenOptions.UseHttps();
                    if (Debugger.IsAttached)
                    {
                        listenOptions.UseConnectionLogging();
                    }
                });
            })

            // Replace with other WSFedBinding or WSHttpWithWindowsAuthAndRoles for other binding types
            .UseStartup<WSHttpUserPassword>();
    }
}
