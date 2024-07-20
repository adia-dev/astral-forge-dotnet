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

    public void ShowUsage()
    {
        Console.WriteLine("Usage: NEEDED_STOCKS A Vaisseau1, B Vaisseau2, ...");
        Console.WriteLine("Example: NEEDED_STOCKS 2 Explorer, 1 Speeder");
        Console.WriteLine("Description: Displays the parts required to produce the specified quantities of spaceships.");
    }
}
