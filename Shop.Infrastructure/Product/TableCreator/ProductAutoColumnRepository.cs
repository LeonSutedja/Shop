using System.Collections.Generic;
using Shop.Infrastructure.TableCreator;

namespace Shop.Infrastructure.Product.TableCreator
{
    public class ProductAutoColumnRepository : AbstractAutoColumnRepository<Product>
    {
        public ProductAutoColumnRepository(IEnumerable<ITableColumn> injectedTableColumns) : base(injectedTableColumns)
        {
        }
    }
}