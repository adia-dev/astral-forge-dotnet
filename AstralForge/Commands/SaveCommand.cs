using AstralForge.Models;
using AstralForge.Utils;

namespace AstralForge.Commands
{
    public class SaveCommand : ICommand
    {
        private readonly string _filePath;

        public SaveCommand(string filePath)
        {
            _filePath = filePath;
        }

        public void Execute(Inventory inventory)
        {
            FileManager.SaveToFile(_filePath, inventory);
            Console.WriteLine($"Inventory saved to {_filePath}.");
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage: SAVE <file_path>");
            Console.WriteLine("Description: Saves the current inventory to the specified file.");
        }
    }
}
