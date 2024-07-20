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
}
