namespace ServiceMessages;

public class RegistrationEventMessage
{
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime RegistrationTime { get; set; }
}