using System.Linq;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;
using Shouldly;
using Unity;
using Xunit;

namespace Shop.Order.Test
{
    public class OrderServiceTest
    {
        private UnityContainer _unityContainer { get; }

        private IOrderService _orderService { get; }

        private IRepository<Customer> _customerRepository { get; }
        private IRepository<Product> _productRepository { get; }

        public OrderServiceTest()
        {
            _unityContainer = new UnityContainer();
            Shop.UnityConfig.RegisterTypes(_unityContainer);
            _orderService = _unityContainer.Resolve<IOrderService>();
            _customerRepository = _unityContainer.Resolve<IRepository<Customer>>();
            _productRepository = _unityContainer.Resolve<IRepository<Product>>();
        }

        [Fact]
        public void CustomerShould_HaveEmptyOrder()
        {
            var firstCustomer = _customerRepository.Get(1);
            var processedOrder = _orderService.GetProcessedCustomerOrders(firstCustomer);

            firstCustomer.ShouldNotBeNull();
            processedOrder.ToList().Count.ShouldBe(0);
        }

        [Fact]
        public void CustomerOrderShould_Succeed()
        {
            var firstCustomer = _customerRepository.All().First();
            firstCustomer.ShouldNotBeNull();

            var firstProduct = _productRepository.All().First(product => product.StockQuantity > 0);

            var order = new Order(firstCustomer, firstProduct, 1);
            _orderService.ProcessOrder(order);

            var processedOrder = _orderService.GetProcessedCustomerOrders(firstCustomer);
            processedOrder.ToList().Count.ShouldBe(1);
        }
    }
}