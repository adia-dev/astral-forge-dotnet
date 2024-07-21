using System.Text;
using AstralForge.Enums;
using AstralForge.Factories;

namespace AstralForge.Models;

[Serializable]
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
    public string GetStockLevels()
    {
        return string.Join(Environment.NewLine, parts.Select(p => $"{p.Value.Quantity} {p.Key}"));
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

    public string GetNeededStocks(Dictionary<string, int> order)
    {
        var neededStocksReport = new StringBuilder();

        var totalParts = new Dictionary<string, Part>();

        foreach (var item in order)
        {
            var spaceship = SpaceshipFactory.CreateSpaceship(item.Key);
            neededStocksReport.AppendLine($"{item.Value} {item.Key}:");

            foreach (var part in spaceship.PartsRequirements)
            {
                var totalQuantity = part.Quantity * item.Value;
                if (totalParts.ContainsKey(part.Name))
                {
                    totalParts[part.Name] = new Part(part.Type, part.Name, totalParts[part.Name].Quantity + totalQuantity);
                }
                else
                {
                    totalParts[part.Name] = new Part(part.Type, part.Name, totalQuantity);
                }
                neededStocksReport.AppendLine($"{totalQuantity} {part.Name}");
            }
        }

        neededStocksReport.AppendLine("Total:");
        foreach (var part in totalParts.Values)
        {
            neededStocksReport.AppendLine($"{part.Quantity} {part.Name}");
        }

        return neededStocksReport.ToString();
    }

    public string GetAssemblyInstructions(Dictionary<string, int> order)
    {
        var instructions = new StringBuilder();

        foreach (var item in order)
        {
            var spaceship = SpaceshipFactory.CreateSpaceship(item.Key);
            for (int i = 0; i < item.Value; i++)
            {
                instructions.AppendLine($"PRODUCING {item.Key}");

                foreach (var part in spaceship.PartsRequirements)
                {
                    instructions.AppendLine($"GET_OUT_STOCK {part.Quantity} {part.Name}");
                }

                instructions.AppendLine($"FINISHED {item.Key}");
            }
        }

        return instructions.ToString();
    }

    public string VerifyOrder(Dictionary<string, int> order)
    {
        foreach (var item in order)
        {
            if (!CheckSpaceshipOrder(item.Key, item.Value))
            {
                return "UNAVAILABLE";
            }
        }

        return "AVAILABLE";
    }

    public string ProduceOrder(Dictionary<string, int> order)
    {
        foreach (var item in order)
        {
            if (!CheckSpaceshipOrder(item.Key, item.Value))
            {
                return $"ERROR Not enough parts to fulfill the order for {item.Value} {item.Key}(s).";
            }
        }

        foreach (var item in order)
        {
            CompleteSpaceshipOrder(item.Key, item.Value);
        }

        return "STOCK_UPDATED";
    }

    private bool CheckSpaceshipOrder(string name, int quantity)
    {
        var spaceship = SpaceshipFactory.CreateSpaceship(name);
        foreach (var part in spaceship.PartsRequirements)
        {
            if (!parts.ContainsKey(part.Name) || parts[part.Name].Quantity < part.Quantity * quantity)
            {
                return false;
            }
        }
        return true;
    }

    private void CompleteSpaceshipOrder(string name, int quantity)
    {
        var spaceship = SpaceshipFactory.CreateSpaceship(name);
        foreach (var part in spaceship.PartsRequirements)
        {
            if (parts.ContainsKey(part.Name))
            {
                parts[part.Name] = new Part(part.Type, part.Name, parts[part.Name].Quantity - part.Quantity * quantity);
            }
        }
        AddSpaceship(name, quantity);
    }
}
