using System.Text.RegularExpressions;

namespace Spaceship
{
    public static class Program
    {
        static Dictionary<string, int> stock = new Dictionary<string, int>
        {
            { "Hull_HE1", 10 },
            { "Hull_HS1", 10 },
            { "Hull_HC1", 10 },
            { "Engine_EE1", 10 },
            { "Engine_ES1", 10 },
            { "Engine_EC1", 10 },
            { "Wings_WE1", 10 },
            { "Wings_WS1", 10 },
            { "Wings_WC1", 10 },
            { "Thruster_TE1", 10 },
            { "Thruster_TS1", 10 },
            { "Thruster_TC1", 10 }
        };

        static Dictionary<string, Dictionary<string, int>> shipRequirements =
            new Dictionary<string, Dictionary<string, int>>
            {
                {
                    "Explorer", new Dictionary<string, int>
                    {
                        { "Hull_HE1", 1 },
                        { "Engine_EE1", 1 },
                        { "Wings_WE1", 1 },
                        { "Thruster_TE1", 1 }
                    }
                },
                {
                    "Speeder", new Dictionary<string, int>
                    {
                        { "Hull_HS1", 1 },
                        { "Engine_ES1", 1 },
                        { "Wings_WS1", 1 },
                        { "Thruster_TS1", 2 }
                    }
                },
                {
                    "Cargo", new Dictionary<string, int>
                    {
                        { "Hull_HC1", 1 },
                        { "Engine_EC1", 1 },
                        { "Wings_WC1", 1 },
                        { "Thruster_TC1", 1 }
                    }
                }
            };

        public static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans l'usine de vaisseaux spatiaux !");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input.ToUpper() == "EXIT") break;

                if (input.ToUpper() == "STOCKS")
                {
                    PrintStocks();
                }
                else if (input.StartsWith("NEEDED_STOCKS"))
                {
                    var arg = input.Substring(14)?.Trim();
                    PrintNeededStocks(arg);
                }
                else if (input.StartsWith("INSTRUCTIONS"))
                {
                    var arg = input.Substring(13)?.Trim();
                    PrintInstructions(arg);
                }
                else if (input.StartsWith("VERIFY"))
                {
                    var arg = input.Substring(7)?.Trim();
                    VerifyOrder(arg);
                }
                else if (input.StartsWith("PRODUCE"))
                {
                    var arg = input.Substring(8)?.Trim();
                    ProduceOrder(arg);
                }
                else
                {
                    Console.WriteLine("Commande inconnue.");
                }
            }
        }

        static void PrintStocks()
        {
            foreach (var item in stock)
            {
                Console.WriteLine($"{item.Value} {item.Key}");
            }
        }

        static void PrintNeededStocks(string args)
        {
            var order = ParseOrder(args);
            var neededStocks = new Dictionary<string, int>();

            foreach (var kvp in order)
            {
                string ship = kvp.Key;
                int quantity = kvp.Value;
                if (!shipRequirements.ContainsKey(ship))
                {
                    Console.WriteLine($"ERROR: '{ship}' is not a recognized spaceship");
                    return;
                }

                Console.WriteLine($"{quantity} {ship}:");

                foreach (var part in shipRequirements[ship])
                {
                    Console.WriteLine($"{part.Value * quantity} {part.Key}");

                    if (neededStocks.ContainsKey(part.Key))
                    {
                        neededStocks[part.Key] += part.Value * quantity;
                    }
                    else
                    {
                        neededStocks[part.Key] = part.Value * quantity;
                    }
                }
            }

            Console.WriteLine("Total:");
            foreach (var part in neededStocks)
            {
                Console.WriteLine($"{part.Value} {part.Key}");
            }
        }

        static void PrintInstructions(string args)
        {
            var order = ParseOrder(args);

            foreach (var kvp in order)
            {
                string ship = kvp.Key;
                int quantity = kvp.Value;
                if (!shipRequirements.ContainsKey(ship))
                {
                    Console.WriteLine($"ERROR: '{ship}' is not a recognized spaceship");
                    return;
                }

                for (int i = 0; i < quantity; i++)
                {
                    Console.WriteLine($"PRODUCING {ship}");

                    foreach (var part in shipRequirements[ship])
                    {
                        Console.WriteLine($"GET_OUT_STOCK {part.Value} {part.Key}");
                    }

                    Console.WriteLine(
                        $"ASSEMBLE TMP1 {shipRequirements[ship].Keys.ElementAt(0)} {shipRequirements[ship].Keys.ElementAt(1)}");
                    Console.WriteLine($"ASSEMBLE TMP1 {shipRequirements[ship].Keys.ElementAt(2)}");
                    if (ship == "Speeder")
                    {
                        Console.WriteLine(
                            $"ASSEMBLE TMP3 [TMP1,{shipRequirements[ship].Keys.ElementAt(2)}] {shipRequirements[ship].Keys.ElementAt(3)}");
                        Console.WriteLine($"ASSEMBLE TMP3 {shipRequirements[ship].Keys.ElementAt(3)}");
                    }

                    Console.WriteLine($"FINISHED {ship}");
                }
            }
        }

        static void VerifyOrder(string args)
        {
            var order = ParseOrder(args);

            foreach (var kvp in order)
            {
                string ship = kvp.Key;
                int quantity = kvp.Value;
                if (!shipRequirements.ContainsKey(ship))
                {
                    Console.WriteLine($"ERROR: '{ship}' is not a recognized spaceship");
                    return;
                }
            }

            Console.WriteLine("AVAILABLE");
        }

        static void ProduceOrder(string args)
        {
            var order = ParseOrder(args);
            var neededStocks = new Dictionary<string, int>();

            foreach (var kvp in order)
            {
                string ship = kvp.Key;
                int quantity = kvp.Value;
                if (!shipRequirements.ContainsKey(ship))
                {
                    Console.WriteLine($"ERROR: '{ship}' is not a recognized spaceship");
                    return;
                }

                foreach (var part in shipRequirements[ship])
                {
                    if (neededStocks.ContainsKey(part.Key))
                    {
                        neededStocks[part.Key] += part.Value * quantity;
                    }
                    else
                    {
                        neededStocks[part.Key] = part.Value * quantity;
                    }
                }
            }

            foreach (var part in neededStocks)
            {
                if (stock[part.Key] < part.Value)
                {
                    Console.WriteLine("UNAVAILABLE");
                    return;
                }
            }

            foreach (var part in neededStocks)
            {
                stock[part.Key] -= part.Value;
            }

            Console.WriteLine("STOCK_UPDATED");
        }

        static Dictionary<string, int> ParseOrder(string args)
        {
            var order = new Dictionary<string, int>();
            var matches = Regex.Matches(args, @"(\d+)\s+(\w+)");

            foreach (Match match in matches)
            {
                int quantity = int.Parse(match.Groups[1].Value);
                string ship = match.Groups[2].Value;

                if (order.ContainsKey(ship))
                {
                    order[ship] += quantity;
                }
                else
                {
                    order[ship] = quantity;
                }
            }

            return order;
        }
    }
}
