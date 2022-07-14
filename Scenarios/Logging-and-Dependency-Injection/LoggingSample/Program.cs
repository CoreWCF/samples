// Project created using the CoreWCF.Templates project template

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

//Register the `Service` class with Dependency Injection, based on it being a single instance
builder.Services.AddSingleton<Service>();

//Register the `Service` class with Dependency Injection, based on it being a scoped instance
builder.Services.AddScoped<Service2>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<Service>();
    serviceBuilder.AddServiceEndpoint<Service, IService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc");

    serviceBuilder.AddService<Service2>();
    serviceBuilder.AddServiceEndpoint<Service2, IService2>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service2.svc");

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
