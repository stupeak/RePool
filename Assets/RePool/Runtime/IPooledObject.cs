namespace Stupeak.RePool
{
    public interface IPooledObject<T>
        where T : class
    {
        IPoolContainer<T> poolContainer { set; }
    }
}