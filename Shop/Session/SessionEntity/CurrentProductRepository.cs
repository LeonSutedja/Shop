using System.Web;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;

namespace Shop.Session.SessionEntity
{
    /// <summary>
    /// As we are currently not persisting into real db, product repository is being persisted and get from the database.
    /// </summary>
    internal class CurrentProductRepository : Session<IRepository<Product>>
    {
        protected override string _suffixSessionName => "-ProductRepository";

        public CurrentProductRepository(HttpContextBase context, Customer currentUser) : base(context, currentUser)
        {
        }

        protected override IRepository<Product> _newEntity() => new ProductRepository();
    }
}