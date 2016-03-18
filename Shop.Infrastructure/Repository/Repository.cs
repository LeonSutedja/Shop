using System.Collections.Generic;
using System.Linq;

namespace Shop.Infrastructure.Repository
{
    public abstract class AbstractId : IId
    {
        public int Id { get; private set; }
        public void SetId(int id) => Id = id;
    }

    /// <summary>
    /// Because this is only a test app, the product repository is not persisted in a db.
    /// Current application persists this inside user session.
    /// </summary>
    public class GenericRepository<T> : IRepository<T> where T : IId
    {
        private int _id;
        private readonly ICollection<T> _collections;
        public GenericRepository()
        {
            _id = 0;
            _collections = new List<T>();
        }
        public void Add(T entity)
        {
            _id++;
            entity.SetId(_id);
            _collections.Add(entity);
        }
        public T Get(int id)
        {
            return _collections.First(item => item.Id == id);
        }
        public void Remove(T entity)
        {
            _id--;
            _collections.Remove(entity);
        }
        public IEnumerable<T> All()
        {
            return _collections.ToList();
        }
    }    
}
