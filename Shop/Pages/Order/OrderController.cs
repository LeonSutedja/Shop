﻿using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using Shop.Infrastructure.Repository;
using Shop.Session;
using Shop.Shared.Controllers;
using System.Web.Mvc;

namespace Shop.Pages.Order
{
    public class OrderController : BaseController
    {
        private readonly IRepository<Product> _productRepository;

        public OrderController(
            IRepository<Customer> customerRepository,
            IRepository<Product> productRepository) : base(customerRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            ViewBag.Order = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var viewModel = new OrderIndexViewModel(_productRepository.All());
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddOrderItem(int productId)
            => Json(new { success = _addOrderItem(productId) });

        private bool _addOrderItem(int productId)
        {
            var productOrdered = _productRepository.Get(productId);
            var customerCurrentOrder = SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Get();
            var currentOrderItem = customerCurrentOrder.GetOrderItemForProductOrdered(productId);

            // Check for enough stock.
            if (currentOrderItem != null)
                if ((currentOrderItem.QtyOrdered + 1) > productOrdered.StockQuantity) return false;
            if (!(productOrdered.StockQuantity > 0)) return false;

            // Add the product
            customerCurrentOrder.AddProduct(productOrdered, 1);
            SessionFacade.CurrentCustomerOrder(HttpContext, CurrentUser).Set(customerCurrentOrder);
            return true;
        }       
    }
}