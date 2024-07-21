using System.IO;
using System.Xml.Serialization;
using AstralForge.Models;

namespace AstralForge.Utils
{
    public class SaveVisitorXml : ISaveVisitor
    {
        private readonly string _filePath;

        public SaveVisitorXml(string filePath)
        {
            _filePath = filePath;
        }

        public void Visit(Inventory inventory)
        {
            var serializer = new XmlSerializer(typeof(Inventory));
            using var writer = new StreamWriter(_filePath);
            serializer.Serialize(writer, inventory);
        }
    }
}