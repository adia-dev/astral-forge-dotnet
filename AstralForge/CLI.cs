using AstralForge.Models;

namespace AstralForge
{
    public class CLI
    {
        private readonly Inventory _inventory;
        private readonly Lexer _lexer;
        private readonly Parser _parser;

        public CLI(Inventory inventory, Lexer lexer, Parser parser)
        {
            _inventory = inventory;
            _lexer = lexer;
            _parser = parser;
        }

        public void Run(string[] args)
        {
            var input = string.Join(" ", args);
            try
            {
                if (input.ToLower().StartsWith("help"))
                {
                    HandleHelpCommand(input);
                    return;
                }

                var tokens = _lexer.Tokenize(input);
                var command = _parser.Parse(tokens);
                command.Execute(_inventory);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                var commandName = input.Split(' ')[0].ToUpper();
                if (Parser.CommandUsages.TryGetValue(commandName, out var showUsage))
                {
                    showUsage();
                }
                else
                {
                    var closestCommand = Parser.GetClosestCommand(commandName);
                    if (closestCommand != null)
                    {
                        Console.WriteLine($"Did you mean '{closestCommand}'?");
                    }
                }
            }
        }

        private void HandleHelpCommand(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                Console.WriteLine("Available commands:");
                foreach (var command in Parser.CommandParsers.Keys)
                {
                    Console.WriteLine($"- {command}");
                }
            }
            else if (parts.Length == 2)
            {
                var commandName = parts[1].ToUpper();
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
    }
}
