using System;
using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Contract;
using CoreWCF.Description;

namespace NetCoreServer
{
    public class Startup
    {
        public const string HOST_IN_WSDL = "localhost";
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;
        public const int NETTCP_PORT = 8089;

        public void ConfigureServices(IServiceCollection services)
        {
            //Enable CoreWCF Services, with metadata (WSDL) support
            services.AddServiceModelServices()
                    .AddServiceModelMetadata()
                    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseServiceModel(builder =>
            {
                // Add the Echo Service
                builder.AddService<EchoService>(serviceOptions =>
                {
                    // Set a base path for all bindings to the service, and WSDL discovery
                    serviceOptions.BaseAddresses.Add(new Uri($"http://{HOST_IN_WSDL}/EchoService"));
                    serviceOptions.BaseAddresses.Add(new Uri($"https://{HOST_IN_WSDL}/EchoService"));
                })
                // Add a BasicHttpBinding endpoint
                .AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(), "/basicHttp")

                // Add WSHttpBinding endpoints
                .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.None), "/wsHttp")
                .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.Transport), "/wsHttp")

                // Add NetTcpBinding
                .AddServiceEndpoint<EchoService, IEchoService>(new NetTcpBinding(), $"/netTcp");

                // Configure WSDL to be available over http & https
                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpGetEnabled = true;
                serviceMetadataBehavior.HttpsGetEnabled = true;
            });
        }
    }

}
