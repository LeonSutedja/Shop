using System.Text;

namespace Shop.Infrastructure.TableCreator
{
    using ColumnFilter;
    using System.Collections.Generic;

    public class TableInput
    {
        public List<TableColumnFilter> Filters { get; set; }

        public List<TableColumnIdentifier> ColumnsRequested { get; set; }

        public TableColumnIdentifier SortBy { get; set; }

        public bool? SortDirectionAsc { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Page Size: " + PageSize);
            stringBuilder.AppendLine("Page Number: " + PageNumber);
            stringBuilder.AppendLine("Sort By: " + SortBy);
            stringBuilder.AppendLine("Is Sort Direction Asc: " + SortDirectionAsc);
            Filters?.ForEach((filter) =>
            {
                stringBuilder.AppendLine("Filter: " + filter.ToString());
            });

            ColumnsRequested?.ForEach((column) =>
            {
                stringBuilder.AppendLine("Column Requested: " + column.ToString());
            });

            return stringBuilder.ToString();
        }
    }
}