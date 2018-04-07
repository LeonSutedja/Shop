using System.ComponentModel;

namespace Shop.Infrastructure.TableCreator.Column
{
    public enum TableColumnFilterStrategy
    {
        // This will be used in TPSR-1536 to describe the filter mechanism (and therefore the UI) used by columns.
        Unknown = 0,

        [Description("A filter that looks for an exact match of any of the strings in a collection.")]
        MultiSelection,

        [Description("A filter that looks for an (exact or partial) match of a string.")]
        FreeText,

        [Description("A filter that matches numbers in a range.")]
        NumericRange,

        [Description("A filter that matches dates in a range.")]
        DateRange,

        [Description("A filter that matches true/false (or N/A) values.")]
        Flag
    }
}