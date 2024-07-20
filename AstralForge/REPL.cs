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

                    var commandName = input?.Split(' ')[0].ToUpper();
                    if (commandName != null && Parser.CommandParsers.TryGetValue(commandName, out var commandParser))
                    {
                        commandParser(new List<Token>()).ShowUsage();
                    }
                    else
                    {
                        var closestCommand = Parser.GetClosestCommand(commandName ?? string.Empty);
                        if (closestCommand != null)
                        {
                            Console.WriteLine($"Did you mean '{closestCommand}'?");
                        }
                    }
                }
            }
        }
    }
}

