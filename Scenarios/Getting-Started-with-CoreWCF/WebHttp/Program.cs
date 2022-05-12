using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;

IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args)
    .UseKestrel(options =>
    {
        options.AllowSynchronousIO = true;
        options.ListenAnyIP(8080);
        options.ListenAnyIP(8443, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    })
    .UseStartup<Startup>();

IWebHost app = builder.Build();
app.Run();
