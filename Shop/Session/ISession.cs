namespace Shop.Session
{
    internal interface ISession<T>
    {
        T Get();

        void Reset();

        void Set(T newEntity);
    }
}