using AstralForge.Commands;
using AstralForge.Enums;
using AstralForge.Extensions;
using AstralForge.Models;

namespace AstralForge;

public class Parser
{
    public static readonly Dictionary<string, Func<List<Token>, ICommand>> CommandParsers = new()
    {
        { "STOCKS", tokens => new StockCommand() },
        { "NEEDED_STOCKS", tokens => new NeededStocksCommand(ParseOrderTokens(tokens)) },
        { "INSTRUCTIONS", tokens => new AssemblyInstructionsCommand(ParseOrderTokens(tokens)) },
        { "VERIFY", tokens => new VerifyCommand(ParseOrderTokens(tokens)) },
        { "PRODUCE", tokens => new ProduceCommand(ParseOrderTokens(tokens)) },
        { "RECEIVE", tokens => new ReceiveCommand(ParsePartsTokens(tokens)) },
        { "HELP", tokens => new HelpCommand(tokens) }
    };

    public static readonly Dictionary<string, Action> CommandUsages = new()
    {
        { "STOCKS", () => new StockCommand().ShowUsage() },
        { "NEEDED_STOCKS", () => new NeededStocksCommand(new Dictionary<string, int>()).ShowUsage() },
        { "INSTRUCTIONS", () => new AssemblyInstructionsCommand(new Dictionary<string, int>()).ShowUsage() },
        { "VERIFY", () => new VerifyCommand(new Dictionary<string, int>()).ShowUsage() },
        { "PRODUCE", () => new ProduceCommand(new Dictionary<string, int>()).ShowUsage() },
        { "RECEIVE", () => new ReceiveCommand(new List<Part>()).ShowUsage() }
    };

    public ICommand Parse(List<Token> tokens)
    {
        if (tokens.Count == 0)
        {
            throw new ArgumentException("No input provided. Please enter a command.");
        }

        var commandToken = tokens[0];
        if (commandToken.Type != TokenType.Command)
        {
            throw new ArgumentException($"Expected a command, but found '{commandToken.Value}'. Please enter a valid command.");
        }

        if (CommandParsers.TryGetValue(commandToken.Value.ToUpper(), out var commandParser))
        {
            return commandParser(tokens);
        }

        var closestCommand = GetClosestCommand(commandToken.Value);
        if (closestCommand != null)
        {
            throw new ArgumentException($"Unknown command: '{commandToken.Value}'. Did you mean '{closestCommand}'?");
        }

        throw new ArgumentException($"Unknown command: '{commandToken.Value}'. Please enter a valid command.");
    }

    public static string? GetClosestCommand(string input)
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
                throw new ArgumentException("Invalid format. Please enter quantities followed by part names.");
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
                throw new ArgumentException("Invalid format. Please separate items with commas.");
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
