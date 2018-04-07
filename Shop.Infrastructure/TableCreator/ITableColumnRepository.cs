using Shop.Infrastructure.TableCreator.Column;
using System.Collections.Generic;

namespace Shop.Infrastructure.TableCreator
{
    public interface ITableColumnRepository<T>
    {
        IList<ITableColumn<T>> GetAllViewColumns();

        ITableColumn<T> GetColumn(TableColumnIdentifier identifier);
    }
}