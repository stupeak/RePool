namespace Stupeak.Pooling
{
    public interface IPooledObject<T>
        where T : class
    {
        IPoolContainer<T> poolContainer { set; }
    }
}