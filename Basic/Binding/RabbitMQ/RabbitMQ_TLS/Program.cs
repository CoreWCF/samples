using System.Net;

var builder = WebApplication.CreateBuilder();
builder.Services.AddServiceModelServices();
builder.Services.AddQueueTransport();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();
var rabbitMqHostUri = new Uri("net.amqps://HOST:PORT/amq.direct/QUEUE_NAME#ROUTING_KEY");
var sslOption = new SslOption
{
    ServerName = rabbitMqHostUri.Host,
    Enabled = true
};
var credentials = new NetworkCredential(ConnectionFactory.DefaultUser, ConnectionFactory.DefaultPass);

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<EchoService>();
    serviceBuilder.AddServiceEndpoint<EchoService, IEchoService>(
        new RabbitMqBinding
        {
            SslOption = sslOption,
            Credentials = credentials,
            QueueConfiguration = new ClassicQueueConfiguration()
        },
        rabbitMqHostUri);
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
