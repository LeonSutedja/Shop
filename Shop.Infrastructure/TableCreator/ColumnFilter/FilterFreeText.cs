namespace Shop.Infrastructure.TableCreator.ColumnFilter
{
    using System;
    using System.Linq.Expressions;

    public enum FilterFreeTextType
    {
        Contains = 0,
        Equals = 1,
        StartsWith = 2,
        EndsWith = 3
    }

    public class FilterFreeText
    {
        public string StringValue { get; set; }

        public FilterFreeTextType Type { get; set; }

        public Expression<Func<string, bool>> GetFilterExpression()
        {
            var stringToCheck = StringValue;
            switch (Type)
            {
                case FilterFreeTextType.Contains:
                    return (stringValue) => stringValue.Contains(stringToCheck);

                case FilterFreeTextType.Equals:
                    return (stringValue) => stringValue == stringToCheck;

                case FilterFreeTextType.StartsWith:
                    return (stringValue) => stringValue.StartsWith(stringToCheck);

                case FilterFreeTextType.EndsWith:
                    return (stringValue) => stringValue.EndsWith(stringToCheck);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return Type.GetDescription() + ": " + StringValue;
        }
    }
}