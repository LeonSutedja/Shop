using Shop.Infrastructure.Customer;
using System.Web;

namespace Shop.Session.SessionEntity
{
    // Example to register a session.
    internal class CurrentCustomerOrder : Session<Order.Order>
    {
        protected override string _suffixSessionName => "-CustomerOrder";

        public CurrentCustomerOrder(HttpContextBase context, Customer currentUser) : base(context, currentUser)
        {
        }

        protected override Order.Order _newEntity() => new Order.Order(_currentUser);
    }
}