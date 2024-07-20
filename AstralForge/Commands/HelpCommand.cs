using AstralForge.Models;

namespace AstralForge.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly List<Token> _tokens;

        public HelpCommand(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public void Execute(Inventory inventory)
        {
            if (_tokens.Count == 1)
            {
                Console.WriteLine("Available commands:");
                foreach (var command in Parser.CommandUsages.Keys)
                {
                    Console.WriteLine($"- {command}");
                }
            }
            else if (_tokens.Count == 2)
            {
                var commandName = _tokens[1].Value.ToUpper();
                if (Parser.CommandUsages.TryGetValue(commandName, out var showUsage))
                {
                    showUsage();
                }
                else
                {
                    Console.WriteLine($"No such command '{commandName}'. Use 'help' to list all available commands.");
                }
            }
            else
            {
                Console.WriteLine("Usage: help [command]");
            }
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage: HELP [command]");
            Console.WriteLine("Description: Displays available commands or usage information for a specific command.");
        }
    }
}