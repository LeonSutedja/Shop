using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;

namespace Shop.Infrastructure.Product
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ITableBuilder<Product> _productTableBuilder;

        public ProductService(IRepository<Product> productRepository, ITableBuilder<Product> productTableBuilder)
        {
            _productRepository = productRepository;
            _productTableBuilder = productTableBuilder;
        }

        public TableOutput GetProducts()
        {
            return _productTableBuilder.PageNumber(1).PageSize(10).IsSortAscending(true).Build();
        }

        public bool UpdateProductDetails(int productId, string name, string description)
        {
            Product product;
            if (!tryGetProduct(productId, out product)) return false;
            product = _productRepository.Get(productId);
            product.SetName(name);
            product.SetDescription(description);
            return true;
        }

        private bool tryGetProduct(int productId, out Product productResult)
        {
            productResult = null;
            var productToGet = _productRepository.Get(productId);
            if (productToGet == null) return false;
            productResult = productToGet;
            return true;
        }
    }

    public class OrderAutoColumnRepository : AbstractAutoColumnRepository<Product>
    {
        public OrderAutoColumnRepository(IEnumerable<ITableColumn> injectedTableColumns) : base(injectedTableColumns)
        {
        }
    }

    public class OrderTableBuilder : TableBuilderGeneric<Product>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ITableColumnRepository<Product> _tableColumnRepository;

        public OrderTableBuilder(IRepository<Product> productRepository, ITableColumnRepository<Product> tableColumnRepository)
        {
            _productRepository = productRepository;
            _tableColumnRepository = tableColumnRepository;
        }

        protected override List<ITableColumn<Product>> GetAllItemsColumnsAvailableForCategory()
            => _tableColumnRepository.GetAllViewColumns().ToList();

        protected override IQueryable<Product> GetItemsToSort()
            => _productRepository.All().AsQueryable();

        protected override IQueryable<Product> GetItemsQueryBaseOnCategory(IQueryable<Product> items)
            => items;

        protected override Dictionary<Type, InitializeColumnSortFunction> InitializeColumnSort()
            => new Dictionary<Type, InitializeColumnSortFunction>();

        protected override Dictionary<Type, InitializeColumnFilterFunction> InitializeColumnFilters()
            => new Dictionary<Type, InitializeColumnFilterFunction>();

        protected override IEnumerable<ITableColumn<Product>> GetInitialColumns(List<ITableColumn<Product>> allColumnsAvailable)
            => allColumnsAvailable;
    }
}