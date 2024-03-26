using Microsoft.Extensions.Logging;

public class CustomerCreatedHandler
{
    private readonly ILogger<CustomerCreatedHandler> _logger;

    public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
    {
        _logger = logger;
    }

    //You can make static this Handle 
    public void Handle(CustomerCreated customerCreated)
    {
        _logger.LogInformation(customerCreated.ToString());
    }
}
