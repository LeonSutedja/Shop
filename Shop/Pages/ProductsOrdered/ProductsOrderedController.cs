using System.Web.Mvc;
using Shop.Shared;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;

namespace Shop.Pages.ProductsOrdered
{
    public class ProductsOrderedController : BaseController
    {

        public ProductsOrderedController(IRepository<Customer> customerRepository) : base(customerRepository)
        {
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
            var productRepository = SessionFacade.CurrentProductRepository(HttpContext, CurrentUser).Get();
            var orderRepository = SessionFacade.CurrentOrderRepository(HttpContext, CurrentUser).Get();
            var order = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var orderService = new OrderService(productRepository, orderRepository);
            orderService.ProcessOrder(order);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Reset();
            return RedirectToAction("Index");
        }
    }
}