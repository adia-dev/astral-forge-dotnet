using AstralForge.Enums;
using AstralForge.Models;

namespace AstralForge;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the spaceship factory!");

        var inventory = new Inventory();

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

        Console.WriteLine("Stock");
        Console.WriteLine(inventory.GetStock());

        inventory.AddSpaceship("Explorer", 2);
        inventory.AddSpaceship("Speeder", 1);

        Console.WriteLine("Stock");
        Console.WriteLine(inventory.GetStock());

        var order = new Dictionary<string, int> { { "Explorer", 1 } };
        Console.WriteLine("Neeeded Stock for an Explorer:");
        Console.WriteLine(inventory.GetNeededStocks(order));

        Console.WriteLine("Assembly Instructions:");
        Console.WriteLine(inventory.GetAssemblyInstructions(order));
    }
}
