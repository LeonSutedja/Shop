using Shop.Infrastructure.Customer;
using System.Collections.Generic;

namespace Shop.Order
{
    public interface IOrderService
    {
        void ProcessOrder(IOrder order);

        IEnumerable<Order> GetProcessedCustomerOrders(Customer customer);
    }
}