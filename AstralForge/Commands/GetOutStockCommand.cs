using AstralForge.Models;

namespace AstralForge.Commands
{
    public class GetOutStockCommand : ICommand
    {
        private readonly string _partName;
        private readonly int _quantity;

        public GetOutStockCommand(string partName, int quantity)
        {
            _partName = partName;
            _quantity = quantity;
        }

        public void Execute(Inventory inventory)
        {
            if (inventory.UsePart(_partName, _quantity))
            {
                Console.WriteLine($"GET_OUT_STOCK {_quantity} {_partName}");
            }
            else
            {
                Console.WriteLine($"ERROR: Not enough {_partName} in stock.");
            }
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage: GET_OUT_STOCK <Quantity> <PartName>");
            Console.WriteLine("Example: GET_OUT_STOCK 1 Hull_HE1");
            Console.WriteLine("Description: Removes the specified quantity of a part from the inventory.");
        }
    }
}