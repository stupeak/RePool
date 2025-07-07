namespace Stupeak.Pooling
{
    public interface IPoolContainer<T>
        where T : class
    {
        //void Prewarm();
        //void Populate();
        
        T Rent();

        void AddExisting(T instance);


        //bool IsAvailable();
        //bool IsInUsed();


        void Return(T instance);

        void Release(T instance);

        void Clear();
    }
}