using System.Web;
using Shop.Infrastructure.Customer;

namespace Shop.Session.SessionEntity
{
    internal class CurrentCustomerOrder : Session<Order.Order>
    {
        protected override string _suffixSessionName => "-CustomerOrder";

        public CurrentCustomerOrder(HttpContextBase context, Customer currentUser) : base(context, currentUser)
        {
        }

        protected override Order.Order _newEntity() => new Order.Order(_currentUser);
    }
}