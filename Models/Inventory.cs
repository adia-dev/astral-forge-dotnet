using AstralForge.Enums;
using AstralForge.Factories;

namespace AstralForge.Models;

public class Inventory
{
    private readonly List<Part> parts = new();
    private readonly List<Spaceship> spaceships = new();

    public void AddPart(PartType type, string name, int quantity)
    {
        var part = parts.FirstOrDefault(p => p.Name == name);
        if (part != null)
        {
            parts.Remove(part);
            parts.Add(new Part(type, name, part.Quantity + quantity));
        }
        else
        {
            parts.Add(new Part(type, name, quantity));
        }
    }

    public void AddSpaceship(string name, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            spaceships.Add(SpaceshipFactory.CreateSpaceship(name));
        }
    }

    public void PrintStock()
    {
        Console.WriteLine("Parts in stock:");
        foreach (var part in parts)
        {
            Console.WriteLine($"{part.Type} - {part.Name}: {part.Quantity}");
        }

        Console.WriteLine("Spaceships in stock:");
        foreach (var spaceship in spaceships)
        {
            Console.WriteLine($"{spaceship.Name}");
        }
    }

    public bool CheckSpaceshipOrder(string name, int quantity)
    {
        var spaceship = SpaceshipFactory.CreateSpaceship(name);
        var requiredParts = spaceship.PartsRequirements;

        foreach (var part in requiredParts)
        {
            var availablePart = parts.FirstOrDefault(p => p.Name == part.Name);
            if (availablePart == null || availablePart.Quantity < part.Quantity * quantity)
            {
                return false;
            }
        }

        return true;
    }
}
