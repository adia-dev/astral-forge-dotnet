using AstralForge.Models;

namespace AstralForge.Commands;

public interface ICommand
{
    void Execute(Inventory inventory);
}

