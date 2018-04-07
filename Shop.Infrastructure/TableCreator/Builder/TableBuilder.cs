using Shop.Infrastructure.TableCreator.Column;
using System.Collections.Generic;

namespace Shop.Infrastructure.TableCreator.Builder
{
    public abstract class TableBuilder<T> : ITableBuilder<T>
    {
        protected List<ColumnFilter.TableColumnFilter> ColumnFilters { get; private set; }

        protected int PageNumberValue { get; private set; }

        protected int PageSizeValue { get; private set; }

        protected bool? IsSortAscendingValue { get; private set; }

        protected TableColumnIdentifier SortByValue { get; private set; }

        protected List<TableColumnIdentifier> ColumnIdsRequestedValue { get; private set; }

        public ITableBuilder<T> PageSize(int pageSize)
        {
            PageSizeValue = pageSize;
            return this;
        }

        public ITableBuilder<T> PageNumber(int pageNumber)
        {
            PageNumberValue = pageNumber;
            return this;
        }

        public ITableBuilder<T> SortBy(TableColumnIdentifier sortBy)
        {
            SortByValue = sortBy;
            return this;
        }

        public ITableBuilder<T> IsSortAscending(bool? isSortAscending)
        {
            IsSortAscendingValue = isSortAscending;
            return this;
        }

        public ITableBuilder<T> ColumnRequested(List<TableColumnIdentifier> colIdsRequested)
        {
            ColumnIdsRequestedValue = colIdsRequested;
            return this;
        }

        public ITableBuilder<T> FilterBy(List<ColumnFilter.TableColumnFilter> columnFilters)
        {
            this.ColumnFilters = columnFilters;
            return this;
        }

        public abstract TableOutput Build();
    }
}