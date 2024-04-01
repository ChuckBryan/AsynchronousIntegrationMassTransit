using MassTransit;
using ServiceMessages;

namespace Service2.Consumers;

public class Consumer2 : IConsumer<RegistrationEventMessage>
{
    private readonly ILogger<Consumer2> _logger;

    public Consumer2(ILogger<Consumer2> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<RegistrationEventMessage> context)
    {
        _logger.LogInformation("Creating Non Verified User Record: {context} @ {datetime}", context.Message.FullName, context.Message.RegistrationTime);

        return Task.CompletedTask;
    }
}