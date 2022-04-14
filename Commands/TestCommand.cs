namespace _141Cleaning.Commands;

class TestCommand : ICommand
{
    public string Regex => "[T|t]est( (.*)|$)";

    public string Execute(string command)
    {
        var testCommand = 
            System.Text.RegularExpressions.Regex.Match(command, Regex)
            .Groups[2]
            .Value;

        return $"Test command: {testCommand}";
    }
}