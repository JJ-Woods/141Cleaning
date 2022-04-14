using System.Text.RegularExpressions;
using _141Cleaning.Commands;
using Microsoft.AspNetCore.Mvc;

namespace _141Cleaning;

[ApiController]
[Route("")]
public class _141CleaningController : ControllerBase
{
    private readonly ILogger<_141CleaningController> _logger;

    private readonly IEnumerable<ICommand> _commandTree;

    public _141CleaningController(ILogger<_141CleaningController> logger)
    {
        _logger = logger;
        _commandTree = new List<ICommand>
        {
            new TestCommand()
        };
    }

    [HttpGet]
    public string Get(string? command)
    {
        if(!(command is string))
            return "Null command";

        var commandMatches = _commandTree.Where(c => {
            return Regex.Match(command, c.Regex, RegexOptions.IgnoreCase).Success;
        });

        if(commandMatches.Count() == 1)
        {
            return commandMatches.Single().Execute(command);
        }
        else
        {
            return $"Invalid command: {command}";
        }
    }
}
