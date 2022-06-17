using System;
using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Contract;

namespace NetCoreServer
{
    public class WSHttpUserPassword
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Enable CoreWCF Services, with metadata (WSDL) support
            services.AddServiceModelServices()
                .AddServiceModelMetadata();
        }

        public void Configure(IApplicationBuilder app)
        {
            var wsHttpBindingWithCredential = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
            wsHttpBindingWithCredential.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            app.UseServiceModel(builder =>
            {
                // Add the Echo Service
                builder.AddService<EchoService>(serviceOptions =>
                {
                    // Set a base address for all bindings to the service, and WSDL discovery
                    serviceOptions.BaseAddresses.Add(new Uri($"http://localhost:{Program.HTTP_PORT}/EchoService"));
                    serviceOptions.BaseAddresses.Add(new Uri($"https://localhost:{Program.HTTPS_PORT}/EchoService"));
                })
                // Add WSHttpBinding endpoints
                .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.None), "/wsHttp")
                .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.Transport), "/wsHttp")
                .AddServiceEndpoint<EchoService, IEchoService>(wsHttpBindingWithCredential, "/wsHttpUserPassword", ep => { ep.Name = "AuthenticatedEP"; });

                builder.ConfigureServiceHostBase<EchoService>(CustomUserNamePasswordValidator.AddToHost);

                // Configure WSDL to be available over http & https
                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
            });
        }
    }
}
