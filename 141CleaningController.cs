using System.Text.RegularExpressions;
using _141Cleaning.Commands;
using Microsoft.AspNetCore.Mvc;

namespace _141Cleaning;

[ApiController]
[Route("")]
public class _141CleaningController : ControllerBase
{
    private readonly ILogger<_141CleaningController> _logger;

    private readonly SlackConnector _slackConnector;

    private readonly IEnumerable<ICommand> _commandTree;

    private readonly IEnumerable<CommandValidation> _commandValidations;

    public _141CleaningController(ILogger<_141CleaningController> logger)
    {
        _logger = logger;
        var client = new HttpClient();
        _slackConnector = new SlackConnector(client);

        _commandTree = new List<ICommand>
        {
            new TestCommand()
        };

        _commandValidations = new List<CommandValidation>
        {
            new CommandValidation("U024K2U11NK", "C037A17E04E"),
            new CommandValidation("U024K2U11NK", "C039VPSH143"),
            new CommandValidation("U025V41QCSF", "C037A17E04E"),
            new CommandValidation("U025V41QCSF", "C039VPSH143")
        };
    }

    [HttpGet]
    public string Get(string?userId, string? channelId, string? command, string? responseUrl)
    {
        var now = DateTime.UtcNow.ToString("o");
        var keystamp = $"(U:{userId}, C:{channelId}, T:{now})";

        if(!ValidateCommand(userId, channelId))
            return respondToUrl(responseUrl, keystamp, "Invalid user");

        if(!(command is string))
            return respondToUrl(responseUrl, keystamp, "Null command");

        var commandMatches = _commandTree.Where(c => {
            return Regex.Match(command, c.Regex, RegexOptions.IgnoreCase).Success;
        });

        if(commandMatches.Count() == 1)
        {
            return respondToUrl(responseUrl, keystamp, commandMatches.Single().Execute(command));
        }
        else
        {
            return respondToUrl(responseUrl, keystamp, $"Invalid command: {command}");
        }
    }

    private bool ValidateCommand(string? userId, string? channelId)
    {
        if
        (
            userId is string 
            && channelId is string
            && _commandValidations.Any(cv => cv.UserId == userId && cv.ChannelId == channelId)
        )
            return true;
        
        //ELSE
        return false;
    }

    private string respondToUrl(string responseUrl, string keystamp, string message)
    {
        var response = $"{keystamp} {message}";
        _slackConnector.RespondToUrl(responseUrl, response);
        return response;
    }
}
