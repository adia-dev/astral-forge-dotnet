using System.Collections.Generic;
using AstralForge.Enums;
using AstralForge.Models;
using NUnit.Framework;

namespace AstralForge.Tests.Models
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

        [Test]
        public void GetAssemblyInstructions_ShouldReturnCorrectInstructions()
        {
            // Arrange
            var inventory = new Inventory();
            var order = new Dictionary<string, int>
            {
                { "Explorer", 1 }
            };

            // Act
            var instructions = inventory.GetAssemblyInstructions(order);

            // Assert
            Assert.That(instructions, Does.Contain("PRODUCING Explorer"));
            Assert.That(instructions, Does.Contain("GET_OUT_STOCK 1 Hull_HE1"));
            Assert.That(instructions, Does.Contain("GET_OUT_STOCK 1 Engine_EE1"));
            Assert.That(instructions, Does.Contain("GET_OUT_STOCK 2 Wings_WE1"));
            Assert.That(instructions, Does.Contain("GET_OUT_STOCK 1 Thruster_TE1"));
            Assert.That(instructions, Does.Contain("FINISHED Explorer"));
        }

        [Test]
        public void VerifyOrder_ShouldReturnAvailableIfEnoughParts()
        {
            // Arrange
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 10);
            inventory.AddPart(PartType.Wings, "Wings_WE1", 20);
            inventory.AddPart(PartType.Thruster, "Thruster_TE1", 10);
            var order = new Dictionary<string, int> { { "Explorer", 1 } };

            // Act
            var result = inventory.VerifyOrder(order);

            // Assert
            Assert.That(result, Is.EqualTo("AVAILABLE"));
        }

        [Test]
        public void VerifyOrder_ShouldReturnUnavailableIfNotEnoughParts()
        {
            // Arrange
            var inventory = new Inventory();
            var order = new Dictionary<string, int> { { "Explorer", 1 } };

            // Act
            var result = inventory.VerifyOrder(order);

            // Assert
            Assert.That(result, Is.EqualTo("UNAVAILABLE"));
        }

        [Test]
        public void ProduceOrder_ShouldUpdateStockIfOrderFulfilled()
        {
            // Arrange
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 10);
            inventory.AddPart(PartType.Wings, "Wings_WE1", 20);
            inventory.AddPart(PartType.Thruster, "Thruster_TE1", 10);
            var order = new Dictionary<string, int> { { "Explorer", 1 } };

            // Act
            var result = inventory.ProduceOrder(order);

            // Assert
            Assert.That(result, Is.EqualTo("STOCK_UPDATED"));
            var stockReport = inventory.GetStock();
            Assert.That(stockReport, Does.Contain("Hull - Hull_HE1: 9"));
            Assert.That(stockReport, Does.Contain("Engine - Engine_EE1: 9"));
            Assert.That(stockReport, Does.Contain("Wings - Wings_WE1: 18"));
            Assert.That(stockReport, Does.Contain("Thruster - Thruster_TE1: 9"));
            Assert.That(stockReport, Does.Contain("Explorer"));
        }

        [Test]
        public void ProduceOrder_ShouldReturnErrorIfNotEnoughParts()
        {
            // Arrange
            var inventory = new Inventory();
            var order = new Dictionary<string, int> { { "Explorer", 1 } };

            // Act
            var result = inventory.ProduceOrder(order);

            // Assert
            Assert.That(result, Does.Contain("ERROR Not enough parts to fulfill the order"));
        }
    }
}
