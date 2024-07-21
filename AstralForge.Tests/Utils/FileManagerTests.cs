using System.IO;
using AstralForge.Enums;
using AstralForge.Models;
using AstralForge.Utils;
using NUnit.Framework;

namespace AstralForge.Tests.Utils
{
    [TestFixture]
    public class FileManagerTests
    {
        [Test]
        public void TestLoadFromFileJson()
        {
            // Arrange
            var filePath = "test_inventory.json";
            File.WriteAllText(filePath, "{\"parts\":[{\"Type\":\"Hull\",\"Name\":\"Hull_HE1\",\"Quantity\":10}]}");

            // Act
            var inventory = FileManager.LoadFromFile(filePath);

            // Assert
            var part = inventory.GetStockLevels().Split('\n')[0];
            Assert.That(part.Trim(), Is.EqualTo("10 Hull_HE1"));
        }

        [Test]
        public void TestSaveToFileJson()
        {
            // Arrange
            var filePath = "test_output.json";
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);

            // Act
            FileManager.SaveToFile(filePath, inventory);

            // Assert
            var json = File.ReadAllText(filePath);
            Assert.That(json, Does.Contain("\"Name\":\"Hull_HE1\",\"Quantity\":10"));
        }

        [Test]
        public void TestLoadFromFileXml()
        {
            // Arrange
            var filePath = "test_inventory.xml";
            File.WriteAllText(filePath,
                "<Inventory><parts><Part><Type>Hull</Type><Name>Hull_HE1</Name><Quantity>10</Quantity></Part></parts></Inventory>");

            // Act
            var inventory = FileManager.LoadFromFile(filePath);

            // Assert
            var part = inventory.GetStockLevels().Split('\n')[0];
            Assert.That(part.Trim(), Is.EqualTo("10 Hull_HE1"));
        }

        [Test]
        public void TestSaveToFileXml()
        {
            // Arrange
            var filePath = "test_output.xml";
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);

            // Act
            FileManager.SaveToFile(filePath, inventory);

            // Assert
            var xml = File.ReadAllText(filePath);
            Assert.That(xml, Does.Contain("<Name>Hull_HE1</Name><Quantity>10</Quantity>"));
        }

        [Test]
        public void TestLoadFromFileText()
        {
            // Arrange
            var filePath = "test_inventory.txt";
            File.WriteAllText(filePath, "10 Hull_HE1\n");

            // Act
            var inventory = FileManager.LoadFromFile(filePath);

            // Assert
            var part = inventory.GetStockLevels().Split('\n')[0];
            Assert.That(part.Trim(), Is.EqualTo("10 Hull_HE1"));
        }

        [Test]
        public void TestSaveToFileText()
        {
            // Arrange
            var filePath = "test_output.txt";
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);

            // Act
            FileManager.SaveToFile(filePath, inventory);

            // Assert
            var text = File.ReadAllText(filePath);
            Assert.That(text, Does.Contain("10 Hull_HE1"));
        }
    }
}