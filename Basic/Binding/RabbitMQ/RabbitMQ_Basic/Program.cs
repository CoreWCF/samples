using System.Net;

var builder = WebApplication.CreateBuilder();
builder.Services.AddServiceModelServices();
builder.Services.AddQueueTransport();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();
var rabbitMqHostUri = new Uri("net.amqp://HOST:PORT/amq.direct/QUEUE_NAME#ROUTING_KEY");
var credentials = new NetworkCredential(ConnectionFactory.DefaultUser, ConnectionFactory.DefaultPass);

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<EchoService>();
    serviceBuilder.AddServiceEndpoint<EchoService, IEchoService>(
        new RabbitMqBinding
        {
            Credentials = credentials,
            QueueConfiguration = new QuorumQueueConfiguration()
        },
        rabbitMqHostUri);
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
