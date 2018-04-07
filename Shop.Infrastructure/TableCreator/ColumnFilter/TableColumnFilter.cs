using Shop.Infrastructure.TableCreator.Column;

namespace Shop.Infrastructure.TableCreator.ColumnFilter
{
    public class TableColumnFilter
    {
        public TableColumnIdentifier ColumnIdentifier { get; set; }

        public FilterMultiselectText MultiselectText { get; set; }

        public FilterFreeText FilterFreeText { get; set; }

        public FilterDateRange FilterDateRange { get; set; }

        public FilterNumberRange NumericRange { get; set; }

        public FilterFlag Boolean { get; set; }

        public override string ToString()
        {
            if (MultiselectText != null)
            {
                return ColumnIdentifier + " - " + MultiselectText;
            }

            if (FilterFreeText != null)
            {
                return ColumnIdentifier + " - " + FilterFreeText;
            }

            if (NumericRange != null)
            {
                return ColumnIdentifier + " - " + NumericRange;
            }

            if (FilterDateRange != null)
            {
                return ColumnIdentifier + " - " + FilterDateRange;
            }

            if (Boolean != null)
            {
                return ColumnIdentifier + " - " + Boolean;
            }

            return ColumnIdentifier + " - No filter passed.";
        }
    }
}