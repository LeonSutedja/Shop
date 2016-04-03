using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Order;
using Shop.Session;
using Shop.Shared.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Shared.Components.ShoppingCartDialog
{
    public class ShoppingCartDialogController : BaseController
    {
        private IOrderService _orderService;

        public ShoppingCartDialogController(IRepository<Customer> customerRepository, IOrderService orderService) : base(customerRepository)
        {
            _orderService = orderService;
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
    }
}