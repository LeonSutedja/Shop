namespace Shop.Infrastructure.TableCreator.ColumnFilter
{
    using System;
    using System.Linq.Expressions;

    public class FilterNumberRange
    {
        public NumericComparisonType ComparisonType { get; set; }

        public decimal From { get; set; }

        public decimal? To { get; set; }

        public Expression<Func<decimal?, bool>> GetFilterExpression()
        {
            switch (ComparisonType)
            {
                case NumericComparisonType.Equals:
                    return (decimalValue) => decimalValue == From;

                case NumericComparisonType.LessThan:
                    return (decimalValue) => decimalValue < From;

                case NumericComparisonType.GreaterThan:
                    return (decimalValue) => decimalValue > From;

                case NumericComparisonType.Between:
                    return (decimalValue) => decimalValue >= From && decimalValue <= To;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Expression<Func<short, bool>> GetShortFilterExpression()
        {
            switch (ComparisonType)
            {
                case NumericComparisonType.Equals:
                    return (shortValue) => shortValue == From;

                case NumericComparisonType.LessThan:
                    return (shortValue) => shortValue < From;

                case NumericComparisonType.GreaterThan:
                    return (shortValue) => shortValue > From;

                case NumericComparisonType.Between:
                    return (shortValue) => shortValue >= From && shortValue <= To;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Expression<Func<int, bool>> GetIntegerFilterExpression()
        {
            switch (ComparisonType)
            {
                case NumericComparisonType.Equals:
                    return (intValue) => intValue == From;

                case NumericComparisonType.LessThan:
                    return (intValue) => intValue < From;

                case NumericComparisonType.GreaterThan:
                    return (intValue) => intValue > From;

                case NumericComparisonType.Between:
                    return (intValue) => intValue >= From && intValue <= To;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return ComparisonType.GetDescription() + " From: " + From + " To: " + To;
        }
    }
}
