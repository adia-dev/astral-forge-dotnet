using AstralForge.Models;

namespace AstralForge.Commands
{
    public class GetMovementsCommand : ICommand
    {

        public GetMovementsCommand()
        {
        }

        public void Execute(Inventory inventory)
        {
            inventory.GetMovements();
        }

        public void ShowUsage()
        {
            Console.WriteLine("Usage: GET_MOVEMENTS");
            Console.WriteLine("Description: Displays the movements of the spaceships and parts in the inventory.");
        }
    }
}