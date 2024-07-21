using AstralForge.Models;

namespace AstralForge.Utils
{
    public interface ISaveVisitor
    {
        void Visit(Inventory inventory);
    }
}