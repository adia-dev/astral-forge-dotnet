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
                if (Parser.CommandParsers.TryGetValue(commandName, out var commandParser))
                {
                    commandParser(new List<Token>()).ShowUsage();
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
    }
}

