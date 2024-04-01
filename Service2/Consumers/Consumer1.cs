using MassTransit;
using ServiceMessages;

namespace Service2.Consumers;

public class Consumer1 : IConsumer<RegistrationEventMessage>
{
    private readonly ILogger<Consumer1> _logger;

    public Consumer1(ILogger<Consumer1> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<RegistrationEventMessage> context)
    {
        _logger.LogInformation("Sending Email Validation Message: {context} @ {datetime}", context.Message.EmailAddress, DateTime.Now);

        return Task.CompletedTask;
    }
}