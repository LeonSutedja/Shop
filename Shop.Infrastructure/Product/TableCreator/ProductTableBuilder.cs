using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
using Shop.Infrastructure.TableCreator.Builder;
using Shop.Infrastructure.TableCreator.Column;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Infrastructure.Product.TableCreator
{
    public class ProductTableBuilder : TableBuilderGeneric<Product>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ITableColumnRepository<Product> _tableColumnRepository;

        public ProductTableBuilder(IRepository<Product> productRepository, ITableColumnRepository<Product> tableColumnRepository)
        {
            _productRepository = productRepository;
            _tableColumnRepository = tableColumnRepository;
        }

        protected override List<ITableColumn<Product>> GetAllTableColumnsForTable()
            => _tableColumnRepository.GetAllViewColumns().ToList();

        protected override IQueryable<Product> GetItemsToSort()
            => _productRepository.All().AsQueryable();

        protected override IQueryable<Product> GetItemsQuery(IQueryable<Product> items)
            => items;

        protected override Dictionary<Type, InitializeColumnSortFunction> InitializeColumnSort()
            => new Dictionary<Type, InitializeColumnSortFunction>();

        protected override Dictionary<Type, InitializeColumnFilterFunction> InitializeColumnFilters()
            => new Dictionary<Type, InitializeColumnFilterFunction>();

        protected override IEnumerable<ITableColumn<Product>> GetInitialColumns(List<ITableColumn<Product>> allColumnsAvailable)
            => allColumnsAvailable;
    }
}