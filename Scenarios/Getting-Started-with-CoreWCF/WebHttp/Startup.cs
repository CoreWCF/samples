using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Xml;
using WebHttp;

internal sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelWebServices(o =>
        {
            o.Title = "Test API";
            o.Version = "1";
            o.Description = "API Description";
            o.TermsOfService = new("http://example.com/terms");
            o.ContactName = "Contact";
            o.ContactEmail = "support@example.com";
            o.ContactUrl = new("http://example.com/contact");
            o.ExternalDocumentUrl = new("http://example.com/doc.pdf");
            o.ExternalDocumentDescription = "Documentation";
        });

        services.AddSingleton(new SwaggerOptions());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<SwaggerMiddleware>();
        app.UseSwaggerUI();

        app.UseServiceModel(builder =>
        {
            var readerQuoates = new XmlDictionaryReaderQuotas
            {
                MaxBytesPerRead = 4096,
                MaxDepth = 32,
                MaxArrayLength = 16384,
                MaxStringContentLength = 16384,
                MaxNameTableCharCount = 16384
            };

            builder.AddService<WebApi>();
            builder.AddServiceWebEndpoint<WebApi, IWebApi>(new WebHttpBinding
            {
                MaxReceivedMessageSize = 5242880,
                MaxBufferSize = 65536,
                ReaderQuotas = readerQuoates
            }, "api", behavior =>
            {
                behavior.HelpEnabled = true;
                behavior.AutomaticFormatSelectionEnabled = true;
            });

            builder.AddServiceWebEndpoint<WebApi, IWebApi>(new WebHttpBinding
            {
                Security = new WebHttpSecurity
                {
                    Mode = WebHttpSecurityMode.Transport
                },
                MaxReceivedMessageSize = 5242880,
                MaxBufferSize = 65536,
                ReaderQuotas = readerQuoates
            }, "api", behavior =>
            {
                behavior.HelpEnabled = true;
                behavior.AutomaticFormatSelectionEnabled = true;
            });
        });
    }
}