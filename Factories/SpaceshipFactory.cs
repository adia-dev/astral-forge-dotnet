using AstralForge.Enums;
using AstralForge.Models;

namespace AstralForge.Factories;

public static class SpaceshipFactory
{
    private static readonly Dictionary<string, List<Part>> spaceshipTemplates = new()
    {
        {
            "Explorer", new List<Part>
            {
                new Part(PartType.Hull, "Hull_HE1", 1),
                new Part(PartType.Engine, "Engine_EE1", 1),
                new Part(PartType.Wings, "Wings_WE1", 2),
                new Part(PartType.Thruster, "Thruster_TE1", 1)
            }
        },
        {
            "Speeder", new List<Part>
            {
                new Part(PartType.Hull, "Hull_HS1", 1),
                new Part(PartType.Engine, "Engine_ES1", 1),
                new Part(PartType.Wings, "Wings_WS1", 2),
                new Part(PartType.Thruster, "Thruster_TS1", 2)
            }
        },
        {
            "Cargo", new List<Part>
            {
                new Part(PartType.Hull, "Hull_HC1", 1),
                new Part(PartType.Engine, "Engine_EC1", 1),
                new Part(PartType.Wings, "Wings_WC1", 2),
                new Part(PartType.Thruster, "Thruster_TC1", 1)
            }
        }
    };

    public static Spaceship CreateSpaceship(string type)
    {
        if (!spaceshipTemplates.ContainsKey(type))
        {
            throw new ArgumentException($"Spaceship type '{type}' is not recognized.");
        }

        return new Spaceship(type, new List<Part>(spaceshipTemplates[type]));
    }
}

