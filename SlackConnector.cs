namespace _141Cleaning;

public class SlackConnector
{
    private readonly HttpClient _client;

    public SlackConnector(HttpClient client)
    {
        _client = client;
    }

    public void RespondToUrl(string responseUrl, string message)
    {
        if(string.IsNullOrEmpty(responseUrl))
            return;

        var values = new Dictionary<string, string>
        {
            {"text", message}
        };

        var payload = new FormUrlEncodedContent(values);

        var response = _client.PostAsync(responseUrl, payload);
    }
}