using System.Linq;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
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

        [Fact]
        public void AddProduct_Should_AddProductToRepository()
        {
            var newProductName = "ProductName 123";
            var newProductDescription = "Test Product To Be Adde";

            var success = _productService.AddProduct(newProductName, newProductDescription);

            success.ShouldBeTrue();

            var productAfterUpdate = _productRepository.All().First(product => product.Name == newProductName);
            productAfterUpdate.ShouldNotBeNull();
            productAfterUpdate.Name.ShouldBe(newProductName);
            productAfterUpdate.Description.ShouldBe(newProductDescription);
        }

        [Fact]
        public void GetProducts_Should_GetProductsTableOutput()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var tableInput = new TableInput
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var tableOutput = _productService.GetProducts(tableInput);
            tableOutput.ShouldNotBeNull();
            tableOutput.Rows.Count.ShouldBeLessThanOrEqualTo(pageSize);
            tableOutput.Rows.Count.ShouldBeGreaterThan(0);
            tableOutput.Columns.Count.ShouldBeGreaterThan(0);
            tableOutput.TotalProductOffersCount.ShouldBeGreaterThan(0);
        }
    }
}