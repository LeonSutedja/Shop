using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;
using Shop.Shared.Controllers;
using Shop.Infrastructure.Product;

namespace Shop.Pages.MyOrder
{
    public class MyOrderController : BaseController
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Shop.Order.Order> _orderRepository;

        public MyOrderController(IRepository<Customer> customerRepository,
            IRepository<Product> productRepository, IRepository<Shop.Order.Order> orderRepository) : base(customerRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            var pendingOrders = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var currentCustomerOrders = new OrderService(_productRepository, _orderRepository).GetProcessedCustomerOrders(CurrentUser);
            var viewModel = new MyOrderIndexViewModel(currentCustomerOrders, pendingOrders);
            return View(viewModel);
        }
    }
}