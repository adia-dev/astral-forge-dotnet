namespace AstralForge.Utils
{
    using System.Xml.Serialization;
    using System.Text.Json;
    using System.IO;
    using AstralForge.Models;
    using AstralForge.Enums;

    public class FileManager
    {
        public static Inventory LoadFromFile(string filePath)
        {
            var inventory = new Inventory();
            var fileExtension = Path.GetExtension(filePath);

            switch (fileExtension)
            {
                case ".json":
                    inventory = LoadFromJson(filePath);
                    break;
                case ".xml":
                    inventory = LoadFromXml(filePath);
                    break;
                case ".txt":
                    inventory = LoadFromText(filePath);
                    break;
                default:
                    throw new NotSupportedException("File format not supported.");
            }

            return inventory;
        }

        private static Inventory LoadFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Inventory>(json);
        }

        private static Inventory LoadFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(Inventory));
            using var reader = new StreamReader(filePath);
            return (Inventory)serializer.Deserialize(reader);
        }

        private static Inventory LoadFromText(string filePath)
        {
            var inventory = new Inventory();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var quantity = int.Parse(parts[0]);
                var name = parts[1];
                inventory.AddPart(PartType.Unknown, name, quantity);
            }

            return inventory;
        }

        public static void SaveToFile(string filePath, Inventory inventory)
        {
            var fileExtension = Path.GetExtension(filePath);

            switch (fileExtension)
            {
                case ".json":
                    SaveToJson(filePath, inventory);
                    break;
                case ".xml":
                    SaveToXml(filePath, inventory);
                    break;
                case ".txt":
                    SaveToText(filePath, inventory);
                    break;
                default:
                    throw new NotSupportedException("File format not supported.");
            }
        }

        private static void SaveToJson(string filePath, Inventory inventory)
        {
            var json = JsonSerializer.Serialize(inventory);
            File.WriteAllText(filePath, json);
        }

        private static void SaveToXml(string filePath, Inventory inventory)
        {
            var serializer = new XmlSerializer(typeof(Inventory));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, inventory);
        }

        private static void SaveToText(string filePath, Inventory inventory)
        {
            var lines = inventory.GetStockLevels().Split(Environment.NewLine);
            File.WriteAllLines(filePath, lines);
        }
    }
}
