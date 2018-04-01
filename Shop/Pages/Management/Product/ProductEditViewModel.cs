using Shop.Infrastructure.Interfaces;
using Shop.Shared.Models.CommandHandler;
using System.ComponentModel.DataAnnotations;

namespace Shop.Pages.Management.Product
{
    public class ProductEditViewModel
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }

        public ProductEditViewModel()
        {
        }
    }

    public class ProductEditViewModelHandler : ICommandHandler<ProductEditViewModel, bool>
    {
        private readonly IProductService _productService;

        public ProductEditViewModelHandler(IProductService productService)
        {
            _productService = productService;
        }

        public bool Handle(ProductEditViewModel model, int userId)
            => _productService.UpdateProductDetails(model.id, model.name, model.description);
    }
}