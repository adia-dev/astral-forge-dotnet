using AstralForge.Models;

namespace AstralForge.Commands;

public class StockCommand : ICommand
{
    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.GetStock());
    }
}

