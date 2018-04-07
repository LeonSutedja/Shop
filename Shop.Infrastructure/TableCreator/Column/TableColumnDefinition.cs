using System.Collections.Generic;

namespace Shop.Infrastructure.TableCreator.Column
{
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