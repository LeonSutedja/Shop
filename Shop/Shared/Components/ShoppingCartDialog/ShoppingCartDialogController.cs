using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;
using Shop.Shared.Controllers;
using System.Web.Mvc;

namespace Shop.Shared.Components.ShoppingCartDialog
{
    public class ShoppingCartDialogController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IRepository<Product> _productRepository;

        public ShoppingCartDialogController(IRepository<Customer> customerRepository, IRepository<Product> productRepository, IOrderService orderService) : base(customerRepository)
        {
            _orderService = orderService;
            _productRepository = productRepository;
        }

        [HttpPost]
        public JsonResult ClearOrder()
        {
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Reset();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult ProcessOrder()
        {
            var order = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            _orderService.ProcessOrder(order);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Reset();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult RemoveOrderItem(int productId)
            => Json(new { success = _removeOrderItem(productId) });

        private bool _removeOrderItem(int productId)
        {
            var productOrdered = _productRepository.Get(productId);
            var customerCurrentOrder = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var currentOrderItem = customerCurrentOrder.GetOrderItemForProductOrdered(productId);

            // Check if there's an order
            if(currentOrderItem == null)
                return false;

            // Add the product
            customerCurrentOrder.RemoveProduct(productOrdered, 1);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Set(customerCurrentOrder);
            return true;
        }
    }
}