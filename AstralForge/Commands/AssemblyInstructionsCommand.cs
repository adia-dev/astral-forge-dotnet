using AstralForge.Models;

namespace AstralForge.Commands;

public class AssemblyInstructionsCommand : ICommand
{
    private readonly Dictionary<string, int> _order;

    public AssemblyInstructionsCommand(Dictionary<string, int> order)
    {
        _order = order;
    }

    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.GetAssemblyInstructions(_order));
    }
}
