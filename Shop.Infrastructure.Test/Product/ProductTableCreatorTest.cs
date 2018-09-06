using Autofac;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.TableCreator;
using Shouldly;
using System.Linq;
using Xunit;

namespace Shop.Infrastructure.Test.Product
{
    public class ProductTableCreatorTest
    {
        private IProductService _productService { get; }
        private ITableColumnRepository<Infrastructure.Product.Product> _productTableColumnRepository { get; }

        public ProductTableCreatorTest()
        {
            var autofacConfig = new AutofacConfig();
            var container = autofacConfig.InitiateAutofacContainerBuilder();
            _productService = container.Resolve<IProductService>();
            _productTableColumnRepository = container.Resolve<ITableColumnRepository<Infrastructure.Product.Product>>();
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