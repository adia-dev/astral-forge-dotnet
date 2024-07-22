using AstralForge.Models;
using AstralForge.Utils;

namespace AstralForge.Commands
{
    public class LoadCommand : ICommand
    {
        private readonly string _filePath;

        public LoadCommand(string filePath)
        {
            _filePath = filePath;
        }

        public void Execute(Inventory inventory)
        {
            var loadedInventory = FileManager.LoadFromFile(_filePath);
            inventory.Merge(loadedInventory);
            Console.WriteLine($"Inventory loaded from {_filePath}.");
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage: LOAD <file_path>");
            Console.WriteLine("Description: Loads the inventory from the specified file.");
        }
    }
}
