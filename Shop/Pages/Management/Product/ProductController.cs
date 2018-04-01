using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Shared.Controllers;
using Shop.Shared.Models.CommandHandler;
using System.Web.Mvc;
using Shop.Infrastructure.Interfaces;

namespace Shop.Pages.Management.Product
{
    public class ProductController : BaseController
    {
        private readonly IRepository<Infrastructure.Product.Product> _productRepository;
        private readonly IProductService _productService;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public ProductController(
            IProductService productService,
            IRepository<Customer> customerRepository,
            IRepository<Infrastructure.Product.Product> productRepository,
            ICommandHandlerFactory commandHandlerFactory) : base(customerRepository)
        {
            _productService = productService;
            _commandHandlerFactory = commandHandlerFactory;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var tableOutput = _productService.GetProducts();
            return View(tableOutput);
        } 

        [HttpPost]
        public ActionResult UpdateProduct(ProductEditViewModel viewModel)
        {
            var handler = _commandHandlerFactory.GetCommandHandler<ProductEditViewModel, bool>();
            var isSuccess = handler.Handle(viewModel, CurrentUser.Id);
            return RedirectToAction("Index");
        }
    }
}