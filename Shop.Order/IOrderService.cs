using System.Collections.Generic;
using Shop.Infrastructure.Customer;

namespace Shop.Order
{
    public interface IOrderService
    {
        void ProcessOrder(IOrder order);
        IEnumerable<Order> GetProcessedCustomerOrders(Customer customer);
    }
}
