using System.Web.Mvc;
using Shop.Shared;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;

namespace Shop.Pages.MyOrder
{
    public class MyOrderController : BaseController
    {
        public MyOrderController(IRepository<Customer> customerRepository) : base(customerRepository)
        {
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            var orderRepository = SessionFacade.CurrentOrderRepository(HttpContext, CurrentUser).Get();
            var productRepository = SessionFacade.CurrentProductRepository(HttpContext, CurrentUser).Get();
            var pendingOrders = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var currentCustomerOrders = new OrderService(productRepository, orderRepository).GetProcessedCustomerOrders(CurrentUser);
            var viewModel = new MyOrderIndexViewModel(currentCustomerOrders, pendingOrders);
            return View(viewModel);
        }
    }
}