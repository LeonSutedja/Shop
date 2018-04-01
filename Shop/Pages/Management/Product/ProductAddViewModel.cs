using Shop.Infrastructure.Interfaces;
using Shop.Shared.Models.CommandHandler;
using System.ComponentModel.DataAnnotations;

namespace Shop.Pages.Management.Product
{
    public class ProductAddViewModel
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }
    }

    public class ProductAddViewModelHandler : ICommandHandler<ProductAddViewModel, bool>
    {
        private readonly IProductService _productService;

        public ProductAddViewModelHandler(IProductService productService)
        {
            _productService = productService;
        }

        public bool Handle(ProductAddViewModel model, int userId)
            => _productService.AddProduct(model.name, model.description);
    }
}