using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;

IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args)
    .UseKestrel(options =>
    {
        options.AllowSynchronousIO = true;
        options.ListenLocalhost(8080);
        options.Listen(address: IPAddress.Loopback, 8081, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    })
    .UseStartup<Startup>();

IWebHost app = builder.Build();
app.Run();
