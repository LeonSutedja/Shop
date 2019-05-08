using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
using Shop.Infrastructure.TableCreator.Column;
using Shop.Shared.Controllers;
using Shop.Shared.Models.CommandHandler;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Shop.Pages.Management.Product
{
    public class ProductController : BaseController
    {
        private readonly ITableColumnRepository<Infrastructure.Product.Product> _productColumnRepository;
        private readonly IProductService _productService;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public ProductController(
            ITableColumnRepository<Infrastructure.Product.Product> productColumnRepository,
            IProductService productService,
            IRepository<Customer> customerRepository,
            ICommandHandlerFactory commandHandlerFactory) : base(customerRepository)
        {
            _productColumnRepository = productColumnRepository;
            _productService = productService;
            _commandHandlerFactory = commandHandlerFactory;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sortBy, string sortDirection, string pageNumber, string pageSize)
        {
            TableColumnIdentifier sortByidentifier = null;
            var sortDirectionAsc = true;
            if (!string.IsNullOrEmpty(sortBy))
            {
                var allProductViewColumns = _productColumnRepository.GetAllViewColumns();
                sortByidentifier = allProductViewColumns
                    .First(
                        col => col.GetColumnDefinition().Identifier.AdditionalData == sortBy)
                    .GetColumnDefinition().Identifier;
            }

            if (sortDirection != null)
                sortDirectionAsc = String.CompareOrdinal(sortDirection, "Desc") != 0;

            pageNumber = pageNumber ?? "1";
            pageSize = pageSize ?? "5";

            var tableInput = new TableInput
            {
                PageNumber = int.Parse(pageNumber),
                PageSize = int.Parse(pageSize),
                SortDirectionAsc = sortDirectionAsc,
                SortBy = sortByidentifier
            };

            var tableOutput = _productService.GetProducts(tableInput);
            var productOutput = new ProductOutput() { Input = tableInput, Output = tableOutput };
            return View(productOutput);
        }

        [HttpPost]
        public ActionResult UpdateProduct(ProductEditViewModel viewModel)
        {
            var handler = _commandHandlerFactory.GetCommandHandler<ProductEditViewModel, bool>();
            var isSuccess = handler.Handle(viewModel, CurrentUser.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddProduct(ProductAddViewModel viewModel)
        {
            var handler = _commandHandlerFactory.GetCommandHandler<ProductAddViewModel, bool>();
            var isSuccess = handler.Handle(viewModel, CurrentUser.Id);
            return RedirectToAction("Index");
        }
    }

    public class ProductOutput
    {
        public TableInput Input { get; set; }
        public TableOutput Output { get; set; }
    }
}