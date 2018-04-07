using Shop.Infrastructure.TableCreator.TableColumns;

namespace Shop.Infrastructure.TableCreator
{
    using System.Collections.Generic;

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