namespace Shop.Infrastructure.TableCreator
{
    using System.Collections.Generic;

    public class TableColumnDefinition
    {
        public string Title { get; set; }

        public TableColumnFilterStrategy FilterStrategy { get; set; }

        public TableColumnIdentifier Identifier { get; set; }

        public List<string> StringFilterValues { get; set; }

        public bool IsFilterDisable { get; set; }

        public bool IsSortDisable { get; set; }
    }
}