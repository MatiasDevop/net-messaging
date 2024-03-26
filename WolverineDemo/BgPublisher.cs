
using Microsoft.Extensions.Hosting;
using Wolverine;

public class BgPublisher : BackgroundService
{
    private readonly IMessageBus _messageBus;

    public BgPublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _messageBus.SendAsync(new CreateCustomer(Guid.NewGuid(), "Harry Potter"));
            await Task.Delay(1000, stoppingToken);
        }
    }
}