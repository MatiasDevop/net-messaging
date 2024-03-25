

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;

var builder = Host.CreateDefaultBuilder();

builder.UseWolverine();

builder.ConfigureServices(x =>
{
    x.AddHostedService<BgPublisher>();
});


var app = builder.Build();

app.Run();


public record CreateCustomer(Guid Id, string FullName);