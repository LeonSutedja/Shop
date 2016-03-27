using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;
using Shop.Shared.Controllers;
using Shop.Infrastructure.Product;

namespace Shop.Pages.ProductsOrdered
{
    public class ProductsOrderedController : BaseController
    {

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Shop.Order.Order> _orderRepository;

        public ProductsOrderedController(IRepository<Customer> customerRepository,
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
            ViewBag.Order = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get(); 
            return View();
        }

        public ActionResult ClearOrder()
        {
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Reset();
            return RedirectToAction("Index");
        }

        public ActionResult ProcessOrder()
        {
            var order = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var orderService = new OrderService(_productRepository, _orderRepository);
            orderService.ProcessOrder(order);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Reset();
            return RedirectToAction("Index");
        }
    }
}