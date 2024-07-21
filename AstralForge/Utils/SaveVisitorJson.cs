using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using AstralForge.Models;

namespace AstralForge.Utils
{
    public class SaveVisitorJson : ISaveVisitor
    {
        private readonly string _filePath;

        public SaveVisitorJson(string filePath)
        {
            _filePath = filePath;
        }

        public void Visit(Inventory inventory)
        {
            var json = JsonSerializer.Serialize(inventory, new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });
            File.WriteAllText(_filePath, json);
        }
    }
}