using AstralForge.Models;

namespace AstralForge.Commands;

public class ProduceCommand : ICommand
{
    private readonly Dictionary<string, int> _order;

    public ProduceCommand(Dictionary<string, int> order)
    {
        _order = order;
    }

    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.ProduceOrder(_order));
    }
}

