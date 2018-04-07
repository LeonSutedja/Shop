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

        [Theory]
        [InlineData("New Product Name", "New Product Description")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("~!@#$%^&*()_+-=[]\\{}|';/.,<>?:\"", "~!@#$%^&*()_+-=[]\\{}|';/.,<>?:\"")]
        public void UpdateProductDetails_Should_UpdateProductDetails(string newProductName, string newProductDescription)
        {
            var firstProductId = _productRepository.All().First().Id;

            var success = _productService.UpdateProductDetails(firstProductId, newProductName, newProductDescription);

            success.ShouldBeTrue();
            var productAfterUpdate = _productRepository.Get(firstProductId);
            productAfterUpdate.Name.ShouldBe(newProductName);
            productAfterUpdate.Description.ShouldBe(newProductDescription);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(2147483600)]
        [InlineData(2147483647)]
        public void AddToStock_Should_AddStock(int numberStockToAdd)
        {
            var firstProduct = _productRepository.All().First();
            var firstProductStock = firstProduct.StockQuantity;
            var firstProductId = firstProduct.Id;

            var success = _productService.AddToStock(firstProductId, numberStockToAdd);

            success.ShouldBeTrue();
            var productAfterUpdate = _productRepository.Get(firstProductId);
            productAfterUpdate.StockQuantity.ShouldBe(firstProductStock + numberStockToAdd);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-2147483647)]
        public void AddToStock_ShouldNot_AddNegativeStock(int numberStockToAdd)
        {
            var firstProduct = _productRepository.All().First();
            var firstProductStock = firstProduct.StockQuantity;
            var firstProductId = firstProduct.Id;

            var success = _productService.AddToStock(firstProductId, numberStockToAdd);

            success.ShouldBeFalse();
            var productAfterUpdate = _productRepository.Get(firstProductId);
            productAfterUpdate.StockQuantity.ShouldBe(firstProductStock);
        }

        [Theory]
        [InlineData("ProductName 123", "Test Product To Be Added")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("~!@#$%^&*()_+-=[]\\{}|';/.,<>?:\"", "~!@#$%^&*()_+-=[]\\{}|';/.,<>?:\"")]
        public void AddProduct_Should_AddProductToRepository(string newProductName, string newProductDescription)
        {
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

        [Fact]
        public void GetProducts_Should_SortDirectionDescending()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var tableInput = new TableInput
            {
                SortBy = new TableColumnIdentifier("Name", TableColumnType.Property),
                SortDirectionAsc = false,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var tableOutput = _productService.GetProducts(tableInput);
            var firstRowName = tableOutput.Rows.First().Cells[0];
            var lastRowName = tableOutput.Rows.Last().Cells[0];
            firstRowName.ShouldBeGreaterThan(lastRowName);
        }
    }
}