using Shop.Infrastructure.Interfaces;
using Shop.Shared.Models.CommandHandler;

namespace Shop.Pages.Management.Product
{
    public class ProductEditViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description{ get; set; }

        public ProductEditViewModel() { }
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