using System.Text;
using AstralForge.Enums;
using AstralForge.Factories;
using AstralForge.Utils;

namespace AstralForge.Models
{
    [Serializable]
    public class Inventory : ISaveAcceptor
    {
        private readonly Dictionary<string, Part> parts = new();
        private readonly List<Spaceship> spaceships = new();
        private readonly List<string> movements = new();
        private readonly Dictionary<string, List<Part>> customSpaceshipTemplates = new();

        public void AddPart(PartType type, string name, int quantity)
        {
            if (parts.ContainsKey(name))
            {
                parts[name] = new Part(type, name, parts[name].Quantity + quantity);
            }
            else
            {
                parts[name] = new Part(type, name, quantity);
            }

            AddMovement($"Added {quantity} {name}");
        }

        public string GetStockLevels()
        {
            return string.Join(Environment.NewLine, parts.Select(p => $"{p.Value.Quantity} {p.Key}"));
        }

        public void GetMovements()
        {
            foreach (var movement in movements)
            {
                Console.WriteLine(movement);
            }
        }

        private void AddMovement(string movement)
        {
            movements.Add(movement);
        }

        public void AddSpaceship(string name, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                spaceships.Add(SpaceshipFactory.CreateSpaceship(name));
            }

            AddMovement($"Added {quantity} {name}");
        }

        public string AddTemplate(string templateName, List<Part> parts)
        {
            if (!SpaceshipFactory.spaceshipTemplates.ContainsKey(templateName)
                && !customSpaceshipTemplates.ContainsKey(templateName))
            {
                return "CANNOT_ADD_TEMPLATE";
            }

            customSpaceshipTemplates.Add(templateName, parts);
            return $"TEMPLATE {templateName} ADDED";
        }

        public string RemoveSpaceships(Dictionary<string, int> order)
        {
            foreach (var item in order)
            {
                if (!CheckSpaceshipOrder(item.Key, item.Value))
                {
                    return $"ERROR Not enough {item.Key} in stock.";
                }
            }

            foreach (var item in order)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    var spaceship = spaceships.FirstOrDefault(s => s.Name == item.Key);
                    if (spaceship != null)
                    {
                        spaceships.Remove(spaceship);
                    }
                }
            }

            AddMovement($"Removed {order.Count} spaceships");

            return "STOCK_UPDATED";
        }

        public string GetStock()
        {
            var stockReport = new StringBuilder();

            stockReport.AppendLine("Parts in stock:");
            foreach (var part in parts.Values)
            {
                stockReport.AppendLine($"{part.Type} - {part.Name}: {part.Quantity}");
            }

            stockReport.AppendLine();
            stockReport.AppendLine("Spaceships in stock:");
            foreach (var spaceship in spaceships)
            {
                stockReport.AppendLine($"{spaceship.Name}");
            }

            return stockReport.ToString();
        }

        public string GetNeededStocks(Dictionary<string, int> order)
        {
            var neededStocksReport = new StringBuilder();

            var totalParts = new Dictionary<string, Part>();

            foreach (var item in order)
            {
                var spaceship = SpaceshipFactory.CreateSpaceship(item.Key);
                neededStocksReport.AppendLine($"{item.Value} {item.Key}:");

                foreach (var part in spaceship.PartsRequirements)
                {
                    var totalQuantity = part.Quantity * item.Value;
                    if (totalParts.ContainsKey(part.Name))
                    {
                        totalParts[part.Name] = new Part(part.Type, part.Name, totalParts[part.Name].Quantity + totalQuantity);
                    }
                    else
                    {
                        totalParts[part.Name] = new Part(part.Type, part.Name, totalQuantity);
                    }
                    neededStocksReport.AppendLine($"{totalQuantity} {part.Name}");
                }
            }

            neededStocksReport.AppendLine("Total:");
            foreach (var part in totalParts.Values)
            {
                neededStocksReport.AppendLine($"{part.Quantity} {part.Name}");
            }

            return neededStocksReport.ToString();
        }

        public string GetAssemblyInstructions(Dictionary<string, int> order)
        {
            var instructions = new StringBuilder();

            foreach (var item in order)
            {
                var spaceship = SpaceshipFactory.CreateSpaceship(item.Key);
                for (int i = 0; i < item.Value; i++)
                {
                    instructions.AppendLine($"PRODUCING {item.Key}");

                    foreach (var part in spaceship.PartsRequirements)
                    {
                        instructions.AppendLine($"GET_OUT_STOCK {part.Quantity} {part.Name}");
                    }

                    instructions.AppendLine($"ASSEMBLE {item.Key} {string.Join(' ', spaceship.PartsRequirements.Select(p => p.Name))}");

                    instructions.AppendLine($"FINISHED {item.Key}");
                }
            }

            return instructions.ToString();
        }

        public string VerifyOrder(Dictionary<string, int> order)
        {
            foreach (var item in order)
            {
                if (!CheckSpaceshipOrder(item.Key, item.Value))
                {
                    return "UNAVAILABLE";
                }
            }

            return "AVAILABLE";
        }

        public string ProduceOrder(Dictionary<string, int> order)
        {
            foreach (var item in order)
            {
                if (!CheckSpaceshipOrder(item.Key, item.Value))
                {
                    return $"ERROR Not enough parts to fulfill the order for {item.Value} {item.Key}(s).";
                }
            }

            foreach (var item in order)
            {
                CompleteSpaceshipOrder(item.Key, item.Value);
            }

            AddMovement($"Produced {order.Count} spaceships");

            return "STOCK_UPDATED";
        }

        private bool CheckSpaceshipOrder(string name, int quantity)
        {
            var spaceship = SpaceshipFactory.CreateSpaceship(name);
            foreach (var part in spaceship.PartsRequirements)
            {
                if (!parts.ContainsKey(part.Name) || parts[part.Name].Quantity < part.Quantity * quantity)
                {
                    return false;
                }
            }
            return true;
        }

        private void CompleteSpaceshipOrder(string name, int quantity)
        {
            var spaceship = SpaceshipFactory.CreateSpaceship(name);
            foreach (var part in spaceship.PartsRequirements)
            {
                if (parts.ContainsKey(part.Name))
                {
                    parts[part.Name] = new Part(part.Type, part.Name, parts[part.Name].Quantity - part.Quantity * quantity);
                    AddMovement($"Used {part.Quantity * quantity} {part.Name}");
                }
            }
            AddSpaceship(name, quantity);

            AddMovement($"Produced {quantity} {name}");
        }

        public bool CheckPartAvailability(string partName)
        {
            return parts.ContainsKey(partName) && parts[partName].Quantity > 0;
        }

        public bool UsePart(string partName, int quantity = 1)
        {
            if (parts.ContainsKey(partName) && parts[partName].Quantity >= quantity)
            {
                parts[partName] = new Part(parts[partName].Type, partName, parts[partName].Quantity - quantity);
                AddMovement($"Used {quantity} {partName}");
                return true;
            }
            return false;
        }

        public void AddAssembledItem(string assemblyName, List<string> partNames)
        {
            parts[assemblyName] = new Part(PartType.Assembled, assemblyName, 1);
            AddMovement($"Assembled {assemblyName} from parts: {string.Join(", ", partNames)}");
        }

        public void Accept(ISaveVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Merge(Inventory other)
        {
            foreach (var part in other.parts.Values)
            {
                AddPart(part.Type, part.Name, part.Quantity);
            }

            foreach (var spaceship in other.spaceships)
            {
                AddSpaceship(spaceship.Name, 1);
            }

            foreach (var template in other.customSpaceshipTemplates)
            {
                if (!customSpaceshipTemplates.ContainsKey(template.Key))
                {
                    customSpaceshipTemplates[template.Key] = template.Value;
                }
                else
                {
                    var existingParts = customSpaceshipTemplates[template.Key];
                    foreach (var part in template.Value)
                    {
                        var existingPart = existingParts.FirstOrDefault(p => p.Name == part.Name);
                        if (existingPart == null)
                        {
                            existingParts.Add(part);
                        }
                        else
                        {
                            existingParts[existingParts.IndexOf(existingPart)] = new Part(existingPart.Type, existingPart.Name, existingPart.Quantity + part.Quantity);
                        }
                    }
                }
            }

            foreach (var movement in other.movements)
            {
                AddMovement(movement);
            }
        }
    }
}
