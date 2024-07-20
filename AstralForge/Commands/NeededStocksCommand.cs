using AstralForge.Models;

namespace AstralForge.Commands;

public class NeededStocksCommand : ICommand
{
    private readonly Dictionary<string, int> _order;

    public NeededStocksCommand(Dictionary<string, int> order)
    {
        _order = order;
    }

    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.GetNeededStocks(_order));
    }
}

