using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;
using Shop.Shared.Controllers;
using System.Web.Mvc;

namespace Shop.Pages.MyOrder
{
    public class MyOrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public MyOrderController(IRepository<Customer> customerRepository, IOrderService orderService) : base(customerRepository)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            var pendingOrders = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            ViewBag.Order = pendingOrders;
            var currentCustomerOrders = _orderService.GetProcessedCustomerOrders(CurrentUser);
            var viewModel = new MyOrderIndexViewModel(currentCustomerOrders, pendingOrders);
            return View(viewModel);
        }
    }
}