namespace _141Cleaning;

public class CommandValidation
{
    public string UserId { get; }

    public string ChannelId { get; }

    public CommandValidation(string userId, string channelId)
    {
        UserId = userId;
        ChannelId = channelId;
    }
}