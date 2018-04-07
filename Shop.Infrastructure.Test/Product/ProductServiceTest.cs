using System.Linq;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shouldly;
using Unity;
using Xunit;

namespace Shop.Infrastructure.Test.Product
{
    public class ProductServiceTest
    {
        private UnityContainer _unityContainer { get; }

        private IProductService _productService { get; }
        private IRepository<Infrastructure.Product.Product> _productRepository { get; }

        public ProductServiceTest()
        {
            _unityContainer = new UnityContainer();
            UnityConfig.RegisterTypes(_unityContainer);
            _productService = _unityContainer.Resolve<IProductService>();
            _productRepository = _unityContainer.Resolve<IRepository<Infrastructure.Product.Product>>();
        }

        [Fact]
        public void UpdateProductDetails_Should_UpdateProductDetails()
        {
            var firstProductId = _productRepository.All().First().Id;
            var newProductName = "New Product Name";
            var newProductDescription = "New Product Description";

            var success = _productService.UpdateProductDetails(firstProductId, newProductName, newProductDescription);

            success.ShouldBeTrue();
            var productAfterUpdate = _productRepository.Get(firstProductId);
            productAfterUpdate.Name.ShouldBe(newProductName);
            productAfterUpdate.Description.ShouldBe(newProductDescription);
        }

        [Fact]
        public void AddToStock_Should_AddStock()
        {
            var firstProduct = _productRepository.All().First();
            var firstProductStock = firstProduct.StockQuantity;
            var firstProductId = firstProduct.Id;

            var numberStockToAdd = 10;

            var success = _productService.AddToStock(firstProductId, numberStockToAdd);

            success.ShouldBeTrue();
            var productAfterUpdate = _productRepository.Get(firstProductId);
            productAfterUpdate.StockQuantity.ShouldBe(firstProductStock + numberStockToAdd);
        }
    }
}