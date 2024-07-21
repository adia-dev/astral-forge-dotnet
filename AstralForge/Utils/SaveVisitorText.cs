using System.IO;
using AstralForge.Models;

namespace AstralForge.Utils
{
    public class SaveVisitorText : ISaveVisitor
    {
        private readonly string _filePath;

        public SaveVisitorText(string filePath)
        {
            _filePath = filePath;
        }

        public void Visit(Inventory inventory)
        {
            var lines = inventory.GetStockLevels().Split(System.Environment.NewLine);
            File.WriteAllLines(_filePath, lines);
        }
    }
}