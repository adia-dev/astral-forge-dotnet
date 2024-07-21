using AstralForge.Models;

namespace AstralForge.Commands;

public class AddTemplateCommand : ICommand
{
    private readonly string _templateName;
    private readonly List<Part> _parts;

    public AddTemplateCommand(Token templateName, List<Part> parts)
    {
        _templateName = templateName.Value;
        _parts = parts;
    }

    public void Execute(Inventory inventory)
    {
        Console.WriteLine(inventory.AddTemplate(_templateName, _parts));
    }

    public void ShowUsage()
    {
        Console.WriteLine("Usage: ADD_TEMPLATE TEMPLATE_NAME, Piece1, [...], PieceN");
        Console.WriteLine("Description: Adds a custom ship template.");
    }
}
