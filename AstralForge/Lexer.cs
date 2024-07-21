using System.Text;
using AstralForge.Enums;
using AstralForge.Models;

namespace AstralForge;

public class Lexer
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "STOCKS", TokenType.Command },
        { "NEEDED_STOCKS", TokenType.Command },
        { "INSTRUCTIONS", TokenType.Command },
        { "VERIFY", TokenType.Command },
        { "PRODUCE", TokenType.Command },
        { "RECEIVE", TokenType.Command },
        { "SEND", TokenType.Command },
        { "GET_MOVEMENTS", TokenType.Command },
        { "HELP", TokenType.Command }
    };

    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int position = 0;

        while (position < input.Length)
        {
            if (char.IsWhiteSpace(input[position]))
            {
                position++;
                continue;
            }

            var token = GetNextToken(input, ref position);
            tokens.Add(token);
        }

        return tokens;
    }

    private Token GetNextToken(string input, ref int position)
    {
        var sb = new StringBuilder();

        if (char.IsDigit(input[position]))
        {
            while (position < input.Length && char.IsDigit(input[position]))
            {
                sb.Append(input[position]);
                position++;
            }
            return new Token(TokenType.Quantity, sb.ToString());
        }

        if (char.IsLetter(input[position]))
        {
            while (position < input.Length && (char.IsLetterOrDigit(input[position]) || input[position] == '_'))
            {
                sb.Append(input[position]);
                position++;
            }

            var value = sb.ToString();
            if (Keywords.TryGetValue(value.ToUpper(), out var tokenType))
            {
                return new Token(tokenType, value);
            }
            return new Token(TokenType.PartName, value);
        }

        if (input[position] == ',')
        {
            position++;
            return new Token(TokenType.Comma, ",");
        }

        sb.Append(input[position]);
        position++;
        return new Token(TokenType.Unknown, sb.ToString());
    }
}
