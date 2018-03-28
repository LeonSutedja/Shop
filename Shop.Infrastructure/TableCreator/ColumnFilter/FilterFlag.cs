using System;
using System.Linq.Expressions;

namespace Shop.Infrastructure.TableCreator.ColumnFilter
{
    public enum FlagFilterType
    {
        None = 0,
        FalseOnly = 1,
        TrueOnly = 2,
        NotSpecified = 3
    }

    public class FilterFlag
    {
        public FlagFilterType FilterType { get; set; }

        public override string ToString()
        {
            return FilterType.GetDescription(); // EnumHelper.GetEnumDescription(FilterType);
        }

        public Expression<Func<bool, bool>> GetFilterExpression()
        {
            switch (FilterType)
            {
                case FlagFilterType.TrueOnly:
                    return (val) => val;
                case FlagFilterType.FalseOnly:
                    return (val) => !val;
                default:
                    return (val) => true;
            }
        }
    }    
}
