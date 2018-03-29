using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Shared.Controllers;
using Shop.Shared.Models.CommandHandler;
using System.Web.Mvc;

namespace Shop.Pages.Management.Product
{
    public class ProductController : BaseController
    {
        private readonly IRepository<Infrastructure.Product.Product> _productRepository;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public ProductController(IRepository<Customer> customerRepository,
            IRepository<Infrastructure.Product.Product> productRepository,
            ICommandHandlerFactory commandHandlerFactory) : base(customerRepository)
        {
            _commandHandlerFactory = commandHandlerFactory;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View(_productRepository.All());

        [HttpPost]
        public ActionResult UpdateProduct(ProductEditViewModel viewModel)
        {
            var handler = _commandHandlerFactory.GetCommandHandler<ProductEditViewModel, bool>();
            var isSuccess = handler.Handle(viewModel, CurrentUser.Id);
            return RedirectToAction("Index");
        }
    }
}