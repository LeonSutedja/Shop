using System.Collections.Generic;

namespace Shop.Infrastructure.Repository
{
    public interface IId
    {
        int Id { get; }

        void SetId(int id);
    }

    public interface IRepository<T> where T : IId
    {
        T Get(int id);

        void Add(T entity);

        void Remove(T entity);

        IEnumerable<T> All();
    }
}