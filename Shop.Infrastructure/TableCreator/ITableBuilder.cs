using Shop.Infrastructure.TableCreator.Column;
using System.Collections.Generic;

namespace Shop.Infrastructure.TableCreator
{
    public interface ITableBuilder<T>
    {
        ITableBuilder<T> ColumnRequested(List<TableColumnIdentifier> colIdsRequested);

        ITableBuilder<T> PageSize(int pageSize);

        ITableBuilder<T> PageNumber(int pageNumber);

        ITableBuilder<T> SortBy(TableColumnIdentifier sortBy);

        ITableBuilder<T> IsSortAscending(bool? isSortAscending);

        ITableBuilder<T> FilterBy(List<ColumnFilter.TableColumnFilter> columnFilters);

        TableOutput Build();
    }
}