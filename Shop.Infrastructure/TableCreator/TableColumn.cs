namespace Shop.Infrastructure.TableCreator
{
    using ColumnFilter;
    using System.Collections.Generic;
    using System.Linq;

    public interface ITableColumn
    {
        string Title { get; }

        TableColumnFilterStrategy FilterStrategy { get; }

        TableColumnIdentifier Identifier { get; }

        IEnumerable<string> StringFilterValues { get; }

        bool IsSortDisable { get; }

        bool IsFilterDisable { get; }

        TableColumnDefinition GetColumnDefinition();
    }

    public interface ITableColumn<T> : ITableColumn
    {
        int? HeadlineSequenceNumber { get; }

        string GetValueAsString(T entity);

        IQueryable<T> ApplyFilter(IQueryable<T> sourceQuery, TableColumnFilter filter);

        IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending);
    }
}