using System.Collections.Generic;
using NUnit.Framework;
using AstralForge.Models;
using AstralForge.Enums;
using AstralForge.Factories;

namespace AstralForge.Tests
{
    [TestFixture]
    public class InventoryTests
    {
        [Test]
        public void AddPart_ShouldAddNewPartToInventory()
        {
            // Arrange
            var inventory = new Inventory();

            // Act
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);

            // Assert
            var stockReport = inventory.GetStock();
            Assert.That(stockReport, Does.Contain("Hull - Hull_HE1: 10"));
        }

        [Test]
        public void AddPart_ShouldUpdateExistingPartQuantity()
        {
            // Arrange
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);

            // Act
            inventory.AddPart(PartType.Hull, "Hull_HE1", 5);

            // Assert
            var stockReport = inventory.GetStock();
            Assert.That(stockReport, Does.Contain("Hull - Hull_HE1: 15"));
        }

        [Test]
        public void AddSpaceship_ShouldAddNewSpaceshipToInventory()
        {
            // Arrange
            var inventory = new Inventory();

            // Act
            inventory.AddSpaceship("Explorer", 2);

            // Assert
            var stockReport = inventory.GetStock();
            Assert.That(stockReport, Does.Contain("Explorer"));
        }

        [Test]
        public void GetStock_ShouldReturnCorrectStock()
        {
            // Arrange
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            inventory.AddSpaceship("Explorer", 1);

            // Act
            var stockReport = inventory.GetStock();

            // Assert
            Assert.That(stockReport, Does.Contain("Hull - Hull_HE1: 10"));
            Assert.That(stockReport, Does.Contain("Explorer"));
        }

        [Test]
        public void GetNeededStocks_ShouldReturnCorrectRequiredParts()
        {
            // Arrange
            var inventory = new Inventory();
            var order = new Dictionary<string, int> { { "Explorer", 1 } };

            // Act
            var neededStocks = inventory.GetNeededStocks(order);

            // Assert
            Assert.That(neededStocks, Does.Contain("1 Explorer:"));
            Assert.That(neededStocks, Does.Contain("1 Hull_HE1"));
            Assert.That(neededStocks, Does.Contain("1 Engine_EE1"));
            Assert.That(neededStocks, Does.Contain("2 Wings_WE1"));
            Assert.That(neededStocks, Does.Contain("1 Thruster_TE1"));
            Assert.That(neededStocks, Does.Contain("Total:"));
            Assert.That(neededStocks, Does.Contain("1 Hull_HE1"));
            Assert.That(neededStocks, Does.Contain("1 Engine_EE1"));
            Assert.That(neededStocks, Does.Contain("2 Wings_WE1"));
            Assert.That(neededStocks, Does.Contain("1 Thruster_TE1"));
        }
    }
}