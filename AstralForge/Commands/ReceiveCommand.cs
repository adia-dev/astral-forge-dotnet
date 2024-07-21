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

    public void ShowUsage()
    {
        Console.WriteLine("Usage: RECEIVE A Part1, B Part2, ...");
        Console.WriteLine("Example: RECEIVE 10 Hull_HE1, 5 Engine_EE1");
        Console.WriteLine("Description: Adds the specified quantities of parts to the inventory.");
    }
}
