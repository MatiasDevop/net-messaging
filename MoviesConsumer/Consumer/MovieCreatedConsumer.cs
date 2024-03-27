using MassTransit;
using MessagingContracts;

namespace MoviesConsumer.Consumer
{
    //This is for testing MassTransit
    public class MovieCreatedConsumer : IConsumer<MovieCreated>
    {
        private readonly ILogger<MovieCreatedConsumer> _logger;
        public MovieCreatedConsumer(ILogger<MovieCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MovieCreated> context)
        {
            _logger.LogInformation(context.Message.ToString());
            return Task.CompletedTask;
        }
    }
}
