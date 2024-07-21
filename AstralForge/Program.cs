using AstralForge.Commands;
using AstralForge.Enums;
using AstralForge.Models;
using AstralForge.Utils;


namespace AstralForge
{
    public static class Program
    {
        private static readonly Dictionary<string, Func<List<Token>, ICommand>> CommandParsers = new()
        {
            { "STOCKS", tokens => new StockCommand() },
            { "NEEDED_STOCKS", tokens => new NeededStocksCommand(ParseOrderTokens(tokens)) },
            { "INSTRUCTIONS", tokens => new AssemblyInstructionsCommand(ParseOrderTokens(tokens)) },
            { "VERIFY", tokens => new VerifyCommand(ParseOrderTokens(tokens)) },
            { "PRODUCE", tokens => new ProduceCommand(ParseOrderTokens(tokens)) },
            { "RECEIVE", tokens => new ReceiveCommand(ParsePartsTokens(tokens)) },
            { "SEND", tokens => new SendCommand(ParseOrderTokens(tokens)) },
            { "GET_MOVEMENTS", tokens => new GetMovementsCommand() },
            { "ADD_TEMPLATE", tokens => new AddTemplateCommand(tokens[0], ParseAddTemplateTokens(tokens)) },
            { "HELP", tokens => new HelpCommand(tokens) } // Add help command
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the spaceship factory!");
            var inventory = new Inventory();
            AddInitialStock(inventory);
            var lexer = new Lexer();
            var parser = new Parser();

            if (args.Length > 0)
            {
                if (args[0] == "load")
                {
                    inventory = FileManager.LoadFromFile(args[1]);
                    Console.WriteLine("Inventory loaded from file.");
                }
                else if (args[0] == "save")
                {
                    FileManager.SaveToFile(args[1], inventory);
                    Console.WriteLine("Inventory saved to file.");
                }
                else
                {
                    var cli = new CLI(inventory, lexer, parser);
                    cli.Run(args);
                }
            }
            else
            {
                var repl = new REPL(inventory, lexer, parser); 
                repl.Start(); // Démarre le REPL
            }
        }


        private static void AddInitialStock(Inventory inventory)
        {
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            inventory.AddPart(PartType.Hull, "Hull_HS1", 10);
            inventory.AddPart(PartType.Hull, "Hull_HC1", 10);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 10);
            inventory.AddPart(PartType.Engine, "Engine_ES1", 10);
            inventory.AddPart(PartType.Engine, "Engine_EC1", 10);
            inventory.AddPart(PartType.Wings, "Wings_WE1", 20);
            inventory.AddPart(PartType.Wings, "Wings_WS1", 20);
            inventory.AddPart(PartType.Wings, "Wings_WC1", 20);
            inventory.AddPart(PartType.Thruster, "Thruster_TE1", 10);
            inventory.AddPart(PartType.Thruster, "Thruster_TS1", 20);
            inventory.AddPart(PartType.Thruster, "Thruster_TC1", 10);
        }

        // ADD_TEMPLATE TEMPLATE_NAME, Piece1, […], PieceN
        private static List<Part> ParseAddTemplateTokens(List<Token> tokens)
        {
            return ParsePartsTokens(tokens.GetRange(2, tokens.Count - 1));
        }

        private static Dictionary<string, int> ParseOrderTokens(List<Token> tokens)
        {
            return ParseTokens(tokens.GetRange(1, tokens.Count - 1), TokenType.PartName);
        }

        private static List<Part> ParsePartsTokens(List<Token> tokens)
        {
            var parsedParts = ParseTokens(tokens.GetRange(1, tokens.Count - 1), TokenType.PartName);
            var parts = new List<Part>();

            foreach (var entry in parsedParts)
            {
                var partType = GetPartType(entry.Key);
                parts.Add(new Part(partType, entry.Key, entry.Value));
            }

            return parts;
        }

        private static Dictionary<string, int> ParseTokens(List<Token> tokens, TokenType expectedNameType)
        {
            var result = new Dictionary<string, int>();

            for (int i = 0; i < tokens.Count; i += 3)
            {
                if (tokens[i].Type != TokenType.Quantity || tokens[i + 1].Type != expectedNameType)
                {
                    throw new ArgumentException("Invalid format.");
                }

                var quantity = int.Parse(tokens[i].Value);
                var name = tokens[i + 1].Value;

                if (result.ContainsKey(name))
                {
                    result[name] += quantity;
                }
                else
                {
                    result[name] = quantity;
                }

                if (i + 2 < tokens.Count && tokens[i + 2].Type != TokenType.Comma)
                {
                    throw new ArgumentException("Invalid format. Missing comma.");
                }
            }

            return result;
        }

        private static PartType GetPartType(string name)
        {
            return name.Contains("Engine") ? PartType.Engine :
                name.Contains("Wings") ? PartType.Wings :
                name.Contains("Thruster") ? PartType.Thruster :
                PartType.Hull;
        }
    }
}