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

    public void ShowUsage()
    {
        Console.WriteLine("Usage: INSTRUCTIONS A Vaisseau1, B Vaisseau2, ...");
        Console.WriteLine("Example: INSTRUCTIONS 1 Explorer");
        Console.WriteLine("Description: Displays the assembly instructions for the specified quantities of spaceships.");
    }
}
