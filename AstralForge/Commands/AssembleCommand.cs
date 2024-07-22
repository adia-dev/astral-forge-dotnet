using AstralForge.Models;

namespace AstralForge.Commands
{
    public class AssembleCommand : ICommand
    {
        private readonly string _assemblyName;
        private readonly List<string> _parts;

        public AssembleCommand(string assemblyName, List<string> parts)
        {
            _assemblyName = assemblyName;
            _parts = parts;
        }

        public void Execute(Inventory inventory)
        {
            foreach (var part in _parts)
            {
                if (!inventory.CheckPartAvailability(part))
                {
                    Console.WriteLine($"ERROR: Part {part} not available in stock.");
                    return;
                }
            }

            foreach (var part in _parts)
            {
                if (!inventory.UsePart(part))
                {
                    Console.WriteLine($"ERROR: Not enough {part} in stock.");
                    return;
                }
            }

            inventory.AddAssembledItem(_assemblyName, _parts);

            Console.WriteLine($"ASSEMBLED {_assemblyName} from parts: {string.Join(", ", _parts)}");
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage: ASSEMBLE <AssemblyName> <Part1> <Part2> ... <PartN>");
            Console.WriteLine("Example: ASSEMBLE Explorer Hull_HE1 Engine_EE1 Wings_WE1 Wings_WE1 Thruster_TE1");
            Console.WriteLine("Description: Assembles the specified parts into a new item.");
        }
    }
}
