using AstralForge.Models;

namespace AstralForge
{
    public class REPL
    {
        private readonly Inventory _inventory;
        private readonly Lexer _lexer;
        private readonly Parser _parser;

        public REPL(Inventory inventory, Lexer lexer, Parser parser)
        {
            _inventory = inventory;
            _lexer = lexer;
            _parser = parser;
        }

        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Entering REPL mode. Type 'exit' to quit.");
            Console.WriteLine("Type 'help' to list all commands or 'help <command>' to get usage information for a specific command.");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ");
                Console.ResetColor();

                var input = Console.ReadLine();
                if (input?.ToLower() == "exit")
                {
                    break;
                }

                if (input?.ToLower().StartsWith("help") == true)
                {
                    HandleHelpCommand(input);
                    continue;
                }

                try
                {
                    var tokens = _lexer.Tokenize(input);
                    var command = _parser.Parse(tokens);
                    command.Execute(_inventory);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Input error: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
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
