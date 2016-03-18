using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;

namespace Shop.Order
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Product> productRepository, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public void ProcessOrder(IOrder order)
        {
            foreach (var orderedItem in order.ProductsOrdered)
            {
                var productOrdered = _productRepository.Get(orderedItem.ProductOrdered.Id);
                if (orderedItem.QtyOrdered > productOrdered.StockQuantity)
                    throw new Exception("Too many products ordered.");
                productOrdered.RemoveStock(orderedItem.QtyOrdered);
            }
            _orderRepository.Add((Order)order);
        }

        public IEnumerable<Order> GetProcessedCustomerOrders(Customer customer) => 
            _orderRepository.All().Where(order => order.CustomerWhoOrdered.Id == customer.Id);
    }
}
