using Microsoft.Extensions.Logging;

public class CreateConsumerHandler
{
    private readonly ILogger<CreateConsumerHandler> _logger;

    public CreateConsumerHandler(ILogger<CreateConsumerHandler> logger)
    {
        _logger = logger;
    }

    //You can make static this Handle 
    public void Handle(CreateCustomer createCustomer)
    {
        _logger.LogInformation(createCustomer.ToString());
    }
}
