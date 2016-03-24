using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Session;
using Shop.Shared.Controllers;

namespace Shop.Pages.Order
{
    public class OrderController : BaseController
    {
        public OrderController(IRepository<Customer> customerRepository) : base(customerRepository)
        {
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            var productRepository = SessionFacade.CurrentProductRepository(HttpContext, CurrentUser).Get();
            ViewBag.Order = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var viewModel = new OrderIndexViewModel(productRepository.All());
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddOrderItem(int productId)
        {
            return (_addOrderItem(productId)) 
                ? Json(new { success = true }) 
                : Json(new {success = false});
        }

        [HttpPost]
        public JsonResult RemoveOrderItem(int productId)
        {
            return (_removeOrderItem(productId))
                ? Json(new { success = true })
                : Json(new { success = false });
        }

        private bool _addOrderItem(int productId)
        {
            var productRepository = SessionFacade.CurrentProductRepository(HttpContext, CurrentUser).Get();
            var productOrdered = productRepository.Get(productId);
            var customerCurrentOrder = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var currentOrderItem = customerCurrentOrder.GetOrderItemForProductOrdered(productId);

            // Check for enough stock.
            if (currentOrderItem != null)
                if ((currentOrderItem.QtyOrdered+1) > productOrdered.StockQuantity) return false;
            if (!(productOrdered.StockQuantity > 0)) return false;

            // Add the product
            customerCurrentOrder.AddProduct(productOrdered, 1);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Set(customerCurrentOrder);
            return true;
        }

        private bool _removeOrderItem(int productId)
        {
            var productRepository = SessionFacade.CurrentProductRepository(HttpContext, CurrentUser).Get();
            var productOrdered = productRepository.Get(productId);
            var customerCurrentOrder = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var currentOrderItem = customerCurrentOrder.GetOrderItemForProductOrdered(productId);

            // Check if there's an order
            if (currentOrderItem == null) return false;

            // Add the product
            customerCurrentOrder.RemoveProduct(productOrdered, 1);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Set(customerCurrentOrder);
            return true;
        }
    }
}