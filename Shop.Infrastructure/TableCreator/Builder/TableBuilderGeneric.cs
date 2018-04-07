using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator.Column;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Infrastructure.TableCreator.Builder
{
    public abstract class TableBuilderGeneric<T> : TableBuilder<T> where T : IId
    {
        // Initial Variables for Processing
        private List<ITableColumn<T>> _allTableColumns;

        protected delegate ITableColumn<T> InitializeColumnFilterFunction(ITableColumn<T> column);

        protected delegate ITableColumn<T> InitializeColumnSortFunction(ITableColumn<T> column);

        public override TableOutput Build()
        {
            //// Setup initial variables for processing
            _allTableColumns = GetAllTableColumnsForTable();

            //// When the default column is null, this should get the initial display columns
            var displayColumnRequested = ColumnIdsRequestedValue == null
                ? GetInitialColumns(_allTableColumns.ToList())
                : GetDisplayColumnRequested(ColumnIdsRequestedValue).ToList();

            var itemsToSort = GetItemsToSort();

            ColumnFilters?.ForEach((filter) =>
            {
                itemsToSort = ProcessFilter(itemsToSort, filter);
            });

            var totalItems = itemsToSort.Count();
            var sortedDisplay = ProcessSortAndPagination(itemsToSort);

            var rowSummary = sortedDisplay
                                    .Select(owc => new TableRow
                                    {
                                        Id = GetId(owc),
                                        Cells = displayColumnRequested.Select(cr => cr.GetValueAsString(owc)).ToList()
                                    }).ToList();

            var displayColumnDtos = displayColumnRequested.Select(col => col.GetColumnDefinition()).ToList();

            return new TableOutput
            {
                Rows = rowSummary,
                Columns = displayColumnDtos,
                TotalProductOffersCount = totalItems
            };
        }

        protected virtual int GetId(T entity)
        {
            return entity.GetType().GetProperty("Id")?.GetValue(entity, null) as int? ?? -1;
        }

        protected abstract List<ITableColumn<T>> GetAllTableColumnsForTable();

        protected abstract IQueryable<T> GetItemsToSort();

        protected abstract IQueryable<T> GetItemsQuery(IQueryable<T> items);

        protected abstract Dictionary<Type, InitializeColumnSortFunction> InitializeColumnSort();

        protected abstract Dictionary<Type, InitializeColumnFilterFunction> InitializeColumnFilters();

        protected abstract IEnumerable<ITableColumn<T>> GetInitialColumns(List<ITableColumn<T>> allColumnsAvailable);

        private IQueryable<T> ProcessFilter(IQueryable<T> productOffers, ColumnFilter.TableColumnFilter tableColumnFilter)
        {
            var offerViewColumn = _allTableColumns.First(column => column.Identifier.Equals(tableColumnFilter.ColumnIdentifier));

            var initializeColumnFilters = InitializeColumnFilters();
            if (initializeColumnFilters.ContainsKey(offerViewColumn.GetType()))
            {
                offerViewColumn = initializeColumnFilters[offerViewColumn.GetType()](offerViewColumn);
            }

            return offerViewColumn.ApplyFilter(productOffers, tableColumnFilter);
        }

        private IEnumerable<T> ProcessSortAndPagination(IQueryable<T> items)
        {
            if (SortByValue == null)
            {
                //***Contains Id, require to re - factor * **/
                return items
                   .OrderBy(item => item.Id)
                   .Skip((PageNumberValue - 1) * PageSizeValue)
                   .Take(PageSizeValue)
                   .ToList();
            }

            var sortByColumn = _allTableColumns
                .First(cd => cd.Identifier.Equals(SortByValue));
            var initializeColumnSort = InitializeColumnSort();
            var type = sortByColumn.GetType().BaseType;

            if (initializeColumnSort.ContainsKey(type))
            {
                sortByColumn = initializeColumnSort[type](sortByColumn);
            }

            // Tell the column to apply its sort, which will give us a list of Offer IDs in display order. Apply pagination to this list.
            var displayItemIds = sortByColumn.ApplySort(items, IsSortAscendingValue.Value)
                                            .Skip((PageNumberValue - 1) * PageSizeValue)
                                            .Take(PageSizeValue)
                                            .ToList();

            // Now that we have one page's worth of IDs we can do a full retrieval of those offers. But we have to re-apply the sort to those full offers!
            //*** Contains Id, require to re-factor ***/
            return items
                .Where(offer => displayItemIds.Contains(offer.Id)) // This will lose the sort order...
                .OrderBy(offer => displayItemIds.IndexOf(offer.Id)); // ... so we have to re-apply it!
        }

        private IEnumerable<ITableColumn<T>> GetDisplayColumnRequested(IEnumerable<TableColumnIdentifier> columnIdentifiersRequested)
        {
            return columnIdentifiersRequested
                .Select(id => _allTableColumns
                    .First(allCol => allCol.Identifier.Equals(id)));
        }
    }
}