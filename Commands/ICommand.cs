namespace _141Cleaning.Commands;

public interface ICommand
{
    string Regex { get; }

    string Execute(string command);
}