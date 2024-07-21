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

    public void ShowUsage()
    {
        Console.WriteLine("Usage: PRODUCE A Vaisseau1, B Vaisseau2, ...");
        Console.WriteLine("Example: PRODUCE 1 Explorer");
        Console.WriteLine("Description: Produces the specified quantities of spaceships, updating the stock accordingly.");
    }
}
