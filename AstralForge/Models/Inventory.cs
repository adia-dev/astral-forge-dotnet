using System.Text;
using AstralForge.Enums;
using AstralForge.Factories;

namespace AstralForge.Models;

public class Inventory
{
    private readonly Dictionary<string, Part> parts = new();
    private readonly List<Spaceship> spaceships = new();

    public void AddPart(PartType type, string name, int quantity)
    {
        if (parts.ContainsKey(name))
        {
            parts[name] = new Part(type, name, parts[name].Quantity + quantity);
        }
        else
        {
            parts[name] = new Part(type, name, quantity);
        }
    }

    public void AddSpaceship(string name, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            spaceships.Add(SpaceshipFactory.CreateSpaceship(name));
        }
    }

    public string GetStock()
    {
        var stockReport = new StringBuilder();

        stockReport.AppendLine("Parts in stock:");
        foreach (var part in parts.Values)
        {
            stockReport.AppendLine($"{part.Type} - {part.Name}: {part.Quantity}");
        }

        stockReport.AppendLine();
        stockReport.AppendLine("Spaceships in stock:");
        foreach (var spaceship in spaceships)
        {
            stockReport.AppendLine($"{spaceship.Name}");
        }

        return stockReport.ToString();
    }
}
