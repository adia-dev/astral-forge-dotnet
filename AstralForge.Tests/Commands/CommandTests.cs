using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using AstralForge.Models;
using AstralForge.Commands;
using AstralForge.Enums;

namespace AstralForge.Tests
{
    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void AssembleCommand_Success()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 1);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 1);
            inventory.AddPart(PartType.Wings, "Wings_WE1", 2);
            inventory.AddPart(PartType.Thruster, "Thruster_TE1", 1);

            var command = new AssembleCommand("Explorer", new List<string> 
            { 
                "Hull_HE1", "Engine_EE1", "Wings_WE1", "Wings_WE1", "Thruster_TE1" 
            });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("ASSEMBLED Explorer from parts:"));
            }
        }

        [Test]
        public void AssembleCommand_PartNotAvailable()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 1);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 1);

            var command = new AssembleCommand("Explorer", new List<string> 
            { 
                "Hull_HE1", "Engine_EE1", "Wings_WE1", "Wings_WE1", "Thruster_TE1" 
            });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("ERROR: Part Wings_WE1 not available in stock."));
            }
        }

        [Test]
        public void VerifyCommand_Success()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 1);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 1);
            inventory.AddPart(PartType.Wings, "Wings_WE1", 2);
            inventory.AddPart(PartType.Thruster, "Thruster_TE1", 1);
            inventory.AddSpaceship("Explorer", 1);

            var order = new Dictionary<string, int> { { "Explorer", 1 } };
            var command = new VerifyCommand(order);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo("AVAILABLE"));
            }
        }

        [Test]
        public void VerifyCommand_Unavailable()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 1);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 1);

            var order = new Dictionary<string, int> { { "Explorer", 1 } };
            var command = new VerifyCommand(order);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo("UNAVAILABLE"));
            }
        }


        [Test]
        public void GetOutStockCommand_NotEnoughStock()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 1);

            var command = new GetOutStockCommand("Hull_HE1", 3);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo("ERROR: Not enough Hull_HE1 in stock."));
            }
        }

        [Test]
        public void GetMovementsCommand_Success()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 5);

            var command = new GetMovementsCommand();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Added 10 Hull_HE1"));
                Assert.That(result, Does.Contain("Added 5 Engine_EE1"));
            }
        }

        [Test]
        public void HelpCommand_Success()
        {
            var inventory = new Inventory();
            var tokens = new List<Token>
            {
                new Token(TokenType.Command, "HELP")
            };
            var command = new HelpCommand(tokens);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Available commands:"));
            }
        }

        [Test]
        public void NeededStocksCommand_Success()
        {
            var inventory = new Inventory();
            var order = new Dictionary<string, int> { { "Explorer", 1 } };
            var command = new NeededStocksCommand(order);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Total:"));
                Assert.That(result, Does.Contain("1 Hull_HE1"));
                Assert.That(result, Does.Contain("1 Engine_EE1"));
                Assert.That(result, Does.Contain("2 Wings_WE1"));
                Assert.That(result, Does.Contain("1 Thruster_TE1"));
            }
        }

        [Test]
        public void StockCommand_Success()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 10);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 5);

            var command = new StockCommand();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Parts in stock:"));
                Assert.That(result, Does.Contain("Hull - Hull_HE1: 10"));
                Assert.That(result, Does.Contain("Engine - Engine_EE1: 5"));
            }
        }

        [Test]
        public void ProduceCommand_Success()
        {
            var inventory = new Inventory();
            inventory.AddPart(PartType.Hull, "Hull_HE1", 1);
            inventory.AddPart(PartType.Engine, "Engine_EE1", 1);
            inventory.AddPart(PartType.Wings, "Wings_WE1", 2);
            inventory.AddPart(PartType.Thruster, "Thruster_TE1", 1);

            var order = new Dictionary<string, int> { { "Explorer", 1 } };
            var command = new ProduceCommand(order);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                command.Execute(inventory);
                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo("STOCK_UPDATED"));
            }
        }
    }
}
