using MassTransit;
using MessagingContracts;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
/* Using MassTransit */
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.SetInMemorySagaRepositoryProvider();

    var assembly = typeof(Program).Assembly;

    x.AddConsumers(assembly);
    x.AddSagaStateMachines(assembly);
    x.AddSagas(assembly);
    x.AddActivities(assembly);

    //x.UsingAmazonSqs((context, cfg) =>
    //{
    //    cfg.Host("eu-west-2", _ => { });
    //    cfg.ConfigureEndpoints(context);
    //});
    //Same here if you want to use RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

//Here is how to use Wolverine and rabbit for the consumer
//builder.Host.UseWolverine(x =>
//{
//    x.ListenToRabbitQueue("movies-queue").UseForReplies();

//    x.UseRabbitMq(c =>
//    {
//        c.HostName = "localhost";
//    }).AutoProvision();
//    // here the same if you want to use in the CLOUD
//    //x.ListenToSqsQueue("movies-queue").UseForReplies();
//    //x.UseAmazonSqsTransport().AutoProvision();

//});

var app = builder.Build();


app.Run();
