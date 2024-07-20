using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using AstralForge.Commands;
using AstralForge.Enums;
using AstralForge.Extensions;
using AstralForge.Models;

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
            { "RECEIVE", tokens => new ReceiveCommand(ParsePartsTokens(tokens)) }
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the spaceship factory!");
            var inventory = new Inventory();
            AddInitialStock(inventory);
            var lexer = new Lexer();
            var parser = new Parser();

            var rootCommand = new RootCommand
            {
                new Argument<string>("input", "Enter the command to execute")
            };

            rootCommand.Handler = CommandHandler.Create<string>(input =>
            {
                try
                {
                    var tokens = lexer.Tokenize(input);
                    var command = parser.Parse(tokens);
                    command.Execute(inventory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    if (CommandParsers.TryGetValue(input.Split(' ')[0].ToUpper(), out var commandParser))
                    {
                        commandParser(new List<Token>()).ShowUsage();
                    }
                    else
                    {
                        var closestCommand = GetClosestCommand(input.Split(' ')[0]);
                        if (closestCommand != null)
                        {
                            Console.WriteLine($"Did you mean '{closestCommand}'?");
                        }
                    }
                }
            });

            rootCommand.Invoke(args);
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

        private static string GetClosestCommand(string input)
        {
            var threshold = 3;
            var closestCommand = CommandParsers.Keys
                .Select(command => new { Command = command, Distance = command.LevenshteinDistance(input) })
                .Where(x => x.Distance <= threshold)
                .OrderBy(x => x.Distance)
                .FirstOrDefault();
            return closestCommand?.Command;
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
