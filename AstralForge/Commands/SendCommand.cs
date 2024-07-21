using AstralForge.Models;

namespace AstralForge.Commands;

public class SendCommand : ICommand
{
    private readonly Dictionary<string, int> _order;

    public SendCommand(Dictionary<string, int> order)
    {
        _order = order;
    }

    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.RemoveSpaceships(_order));
    }

    public void ShowUsage()
    {
        Console.WriteLine("Usage: SEND 1 Vaisseau");
        Console.WriteLine("Description: Sends the specified spaceship to the client.");
    }
}
