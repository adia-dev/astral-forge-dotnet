namespace AstralForge.Utils
{
    public interface ISaveAcceptor
    {
        void Accept(ISaveVisitor visitor);
    }
}