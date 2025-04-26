namespace RD_SimpleDI.Runtime.DI.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}