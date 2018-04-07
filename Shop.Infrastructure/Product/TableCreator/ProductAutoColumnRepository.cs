using Shop.Infrastructure.TableCreator;
using Shop.Infrastructure.TableCreator.Column;
using System.Collections.Generic;

namespace Shop.Infrastructure.Product.TableCreator
{
    public class ProductAutoColumnRepository : AbstractAutoColumnRepository<Product>
    {
        public ProductAutoColumnRepository(IEnumerable<ITableColumn> injectedTableColumns) : base(injectedTableColumns)
        {
        }
    }
}