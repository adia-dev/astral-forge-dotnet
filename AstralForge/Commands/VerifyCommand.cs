using AstralForge.Models;

namespace AstralForge.Commands;

public class VerifyCommand : ICommand
{
    private readonly Dictionary<string, int> _order;

    public VerifyCommand(Dictionary<string, int> order)
    {
        _order = order;
    }

    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.VerifyOrder(_order));
    }

    public void ShowUsage()
    {
        Console.WriteLine("Usage: VERIFY A Vaisseau1, B Vaisseau2, ...");
        Console.WriteLine("Example: VERIFY 1 Explorer");
        Console.WriteLine("Description: Checks if the specified quantities of spaceships can be produced with the current stock.");
    }
}
