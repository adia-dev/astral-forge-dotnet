using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace AstralForge.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Program_ShouldLoadInventory()
        {
            // Arrange
            var inventoryFilePath = "test_inventory.json";
            File.WriteAllText(inventoryFilePath,
                "{\"parts\":[{\"Type\":\"Hull\",\"Name\":\"Hull_HE1\",\"Quantity\":10}]}");

            var projectPath = Path.GetFullPath(@"../../../AstralForge/AstralForge.csproj");

            // Act
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"run --project \"{projectPath}\" load {inventoryFilePath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            var errorOutput = process.StandardError.ReadToEnd();

            process.WaitForExit();

            // Assert
            Assert.That(output, Does.Contain("Inventory loaded from file."));
            if (!string.IsNullOrEmpty(errorOutput))
            {
                Assert.Fail($"Error output: {errorOutput}");
            }
        }
    }
}