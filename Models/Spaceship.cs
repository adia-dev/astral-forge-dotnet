namespace AstralForge.Models;

public class Spaceship
{
    public string Name { get; private set; }
    public List<Part> PartsRequirements { get; private set; }

    public Spaceship(string name, List<Part> partsRequirements)
    {
        Name = name;
        PartsRequirements = partsRequirements;
    }
}
