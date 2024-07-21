using System;
using AstralForge.Factories;
using NUnit.Framework;

namespace AstralForge.Tests.Factories
{
    [TestFixture]
    public class SpaceshipFactoryTests
    {
        [Test]
        public void CreateSpaceship_ShouldReturnValidSpaceship()
        {
            // Arrange
            var name = "Explorer";

            // Act
            var spaceship = SpaceshipFactory.CreateSpaceship(name);

            // Assert
            Assert.That(spaceship, Is.Not.Null);
            Assert.That(spaceship.Name, Is.EqualTo(name));
            Assert.That(spaceship.PartsRequirements, Is.Not.Empty);
        }

        [Test]
        public void CreateSpaceship_InvalidName_ShouldThrowException()
        {
            // Arrange
            var name = "InvalidShip";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => SpaceshipFactory.CreateSpaceship(name));
        }
    }
}