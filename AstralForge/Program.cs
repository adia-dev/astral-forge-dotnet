using AstralForge.Enums;
using AstralForge.Models;

namespace AstralForge;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the spaceship factory!");
        var inventory = new Inventory();
        AddInitialStock(inventory);
        var lexer = new Lexer();
        var parser = new Parser();

        while (true)
        {
            Console.WriteLine("Enter command:");
            var input = Console.ReadLine();
            if (input == null) continue;

            try
            {
                var tokens = lexer.Tokenize(input);
                var command = parser.Parse(tokens);
                command.Execute(inventory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
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
}
