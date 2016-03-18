using System.Web;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;

namespace Shop.Session.SessionEntity
{
    /// <summary>
    /// As we are currently not persisting into real db, product repository is being persisted and get from the database.
    /// </summary>
    internal class CurrentOrderRepository : Session<IRepository<Order.Order>>
    {
        protected override string _suffixSessionName => "-OrderRepository";

        public CurrentOrderRepository(HttpContextBase context, Customer currentUser) : base(context, currentUser)
        {

        }

        protected override IRepository<Order.Order> _newEntity() => new OrderRepository();
    }
}