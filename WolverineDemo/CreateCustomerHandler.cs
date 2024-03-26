using Microsoft.Extensions.Logging;

public class CreateCustomerHandler
{
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(ILogger<CreateCustomerHandler> logger)
    {
        _logger = logger;
    }

    //You can make static this Handle 
    public CustomerCreated Handle(CreateCustomer createCustomer)
    {
        _logger.LogInformation(createCustomer.ToString());
        return new CustomerCreated(createCustomer.Id, createCustomer.FullName);
    }
}
