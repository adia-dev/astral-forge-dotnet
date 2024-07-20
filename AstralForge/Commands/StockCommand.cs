using AstralForge.Models;

namespace AstralForge.Commands;

public class StockCommand : ICommand
{
    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.GetStock());
    }

    public void ShowUsage()
    {
        Console.WriteLine("Usage: STOCKS");
        Console.WriteLine("Description: Displays the current stock of parts and spaceships in the inventory.");
    }
}
