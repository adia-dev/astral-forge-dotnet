using System;
using NUnit.Framework;
using AstralForge.Commands;
using AstralForge.Models;
using System.Collections.Generic;
using System.IO;
using AstralForge.Enums;

namespace AstralForge.Tests.Commands
{
    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void StockCommand_ShouldReturnStockLevels()
        {
            // Arrange
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            var command = new StockCommand();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                command.Execute(inventory);

                // Assert
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Hull - Hull_HE1: 10"));
            }
        }

        [Test]
        public void NeededStocksCommand_ShouldReturnNeededStocks()
        {
            // Arrange
            var inventory = new Inventory();
            var order = new Dictionary<string, int> { { "Explorer", 1 } };
            var command = new NeededStocksCommand(order);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                command.Execute(inventory);

                // Assert
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("1 Explorer:"));
                Assert.That(result, Does.Contain("1 Hull_HE1"));
                Assert.That(result, Does.Contain("1 Engine_EE1"));
                Assert.That(result, Does.Contain("2 Wings_WE1"));
                Assert.That(result, Does.Contain("1 Thruster_TE1"));
            }
        }

        // Add more tests for other commands here...
    }
}