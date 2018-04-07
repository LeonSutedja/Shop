using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.TableCreator;
using Shouldly;
using System.Linq;
using Unity;
using Xunit;

namespace Shop.Infrastructure.Test.Product
{
    public class ProductTableCreatorTest
    {
        private UnityContainer _unityContainer { get; }

        private IProductService _productService { get; }
        private ITableColumnRepository<Infrastructure.Product.Product> _productTableColumnRepository { get; }

        public ProductTableCreatorTest()
        {
            _unityContainer = new UnityContainer();
            UnityConfig.RegisterTypes(_unityContainer);
            _productService = _unityContainer.Resolve<IProductService>();
            _productTableColumnRepository = _unityContainer.Resolve<ITableColumnRepository<Infrastructure.Product.Product>>();
        }

        [Fact]
        public void ProductTableColumnRepository_ShouldHave_Three_Columns()
        {
            var allViewColumns = _productTableColumnRepository.GetAllViewColumns().ToList();
            allViewColumns.Count.ShouldBe(3);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Description")]
        [InlineData("StockQuantity")]
        public void ProductTableColumnRepository_ShouldHave_columnName(string columnName)
        {
            var allViewColumns = _productTableColumnRepository.GetAllViewColumns().ToList();
            var column = allViewColumns.FirstOrDefault(col => col.Identifier.AdditionalData.Contains(columnName));
            column.ShouldNotBeNull();
        }
    }
}