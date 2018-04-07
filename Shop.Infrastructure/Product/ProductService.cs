using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator;
using Shop.Infrastructure.TableCreator.ColumnFilter;

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

        public bool AddProduct(string name, string description)
        {
            try
            {
                var newProduct = new Product(name, 0, description);
                _productRepository.Add(newProduct);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddToStock(int productId, int quantity)
        {
            if (!_tryGetProduct(productId, out var product)) return false;
            product.AddStock(quantity);
            return true;
        }

        public TableOutput GetProducts(TableInput input)
        {
            return _productTableBuilder
                .ColumnRequested(input.ColumnsRequested)
                .FilterBy(input.Filters)
                .PageNumber(input.PageNumber)
                .PageSize(input.PageSize)
                .IsSortAscending(input.SortDirectionAsc)
                .Build();
        }

        public bool UpdateProductDetails(int productId, string name, string description)
        {
            if (!_tryGetProduct(productId, out var product)) return false;
            product = _productRepository.Get(productId);
            product.SetName(name);
            product.SetDescription(description);
            return true;
        }

        private bool _tryGetProduct(int productId, out Product productResult)
        {
            productResult = null;
            var productToGet = _productRepository.Get(productId);
            if (productToGet == null) return false;
            productResult = productToGet;
            return true;
        }
    }

    public class ProductAutoColumnRepository : AbstractAutoColumnRepository<Product>
    {
        public ProductAutoColumnRepository(IEnumerable<ITableColumn> injectedTableColumns) : base(injectedTableColumns)
        {
        }
    }

    public class ProductTableBuilder : TableBuilderGeneric<Product>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ITableColumnRepository<Product> _tableColumnRepository;

        public ProductTableBuilder(IRepository<Product> productRepository, ITableColumnRepository<Product> tableColumnRepository)
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
    
    public class ProductQuantityColumn : ITableColumn<Product>
    {
        public string Title => "Quantity";
        public TableColumnFilterStrategy FilterStrategy => TableColumnFilterStrategy.NumericRange;
        public TableColumnIdentifier Identifier => new TableColumnIdentifier("qty", TableColumnType.Property);
        public IEnumerable<string> StringFilterValues => new List<string>();
        public bool IsSortDisable => false;
        public bool IsFilterDisable => false;

        public TableColumnDefinition GetColumnDefinition()
            => new TableColumnDefinition
            {
                FilterStrategy = FilterStrategy,
                Identifier = Identifier,
                IsFilterDisable = IsFilterDisable,
                IsSortDisable = IsSortDisable,
                StringFilterValues = StringFilterValues.ToList(),
                Title = Title
            };

        public int? HeadlineSequenceNumber => 0;

        public string GetValueAsString(Product entity)
            => entity.StockQuantity.ToString();

        public IQueryable<Product> ApplyFilter(IQueryable<Product> sourceQuery, TableColumnFilter filter)
            => sourceQuery;

        public IQueryable<int> ApplySort(IQueryable<Product> sourceQuery, bool isAscending)
            => sourceQuery.Select(q => q.Id);

    }

    public class Test
    {

    }

    public class TestQuantityColumn : ITableColumn<Test>
    {
        public string Title { get; }
        public TableColumnFilterStrategy FilterStrategy { get; }
        public TableColumnIdentifier Identifier { get; }
        public IEnumerable<string> StringFilterValues { get; }
        public bool IsSortDisable { get; }
        public bool IsFilterDisable { get; }
        public TableColumnDefinition GetColumnDefinition()
        {
            throw new NotImplementedException();
        }

        public int? HeadlineSequenceNumber { get; }
        public string GetValueAsString(Test entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Test> ApplyFilter(IQueryable<Test> sourceQuery, TableColumnFilter filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<int> ApplySort(IQueryable<Test> sourceQuery, bool isAscending)
        {
            throw new NotImplementedException();
        }
    }
}