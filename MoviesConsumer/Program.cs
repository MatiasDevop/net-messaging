using MessagingContracts;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Here is how to use Wolverine and rabbit for the consumer
builder.Host.UseWolverine(x =>
{
    x.ListenToRabbitQueue("movies-queue").UseForReplies();

    x.UseRabbitMq(c =>
    {
        c.HostName = "localhost";
    }).AutoProvision();
    // here the same if you want to use in the CLOUD
    //x.ListenToSqsQueue("movies-queue").UseForReplies();
    //x.UseAmazonSqsTransport().AutoProvision();

});

var app = builder.Build();


app.Run();
