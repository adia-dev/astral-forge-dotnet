using AstralForge.Models;

namespace AstralForge.Commands;

public class ReceiveCommand : ICommand
{
    private readonly List<Part> _parts;

    public ReceiveCommand(List<Part> parts)
    {
        _parts = parts;
    }

    public void Execute(Inventory inventory)
    {
        foreach (var part in _parts)
        {
            inventory.AddPart(part.Type, part.Name, part.Quantity);
        }
        Console.WriteLine("Stock updated with received items.");
    }
}

