namespace Shop.Infrastructure.TableCreator.ColumnFilter
{
    using System.Collections.Generic;
    using System.Linq;

    public class FilterMultiselectText
    {
        public List<string> StringValues { get; set; }

        public override string ToString()
        {
            return string.Join(", ", StringValues);
        }

        internal List<TEnum> GetEnumsOfStringValues<TEnum>()
        {
            return StringValues.Select(EnumHelper.GetEnumFromDescription<TEnum>).ToList();
        }
    }
}