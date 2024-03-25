using MessagingContracts;

namespace MoviesConsumer.Handlers
{
    public class MovieCreateHandler
    {
        private readonly ILogger<MovieCreateHandler> _logger;

        public MovieCreateHandler(ILogger<MovieCreateHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(MovieCreated movieCreated)
        {
            _logger.LogInformation(movieCreated.ToString());//only test porpuses
        }
    }
}
