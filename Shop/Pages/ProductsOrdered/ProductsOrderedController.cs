using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;
using Shop.Shared.Controllers;

namespace Shop.Pages.ProductsOrdered
{
    public class ProductsOrderedController : BaseController
    {
        private readonly IOrderService _orderService;

        public ProductsOrderedController(IRepository<Customer> customerRepository,
            IOrderService orderService) : base(customerRepository)
        {
            _orderService = orderService;
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
            _orderService.ProcessOrder(order);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Reset();
            return RedirectToAction("Index");
        }
    }
}