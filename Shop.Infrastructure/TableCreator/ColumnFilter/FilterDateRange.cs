using System.ComponentModel;
using System.Reflection;

namespace Shop.Infrastructure.TableCreator.ColumnFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static Dictionary<string, string> CreateEnumDescriptionDictionary<TEnum>()
        {
            var newDictionary = new Dictionary<string, string>();
            return newDictionary;
        }

        public static TEnum GetEnumFromDescription<TEnum>(string description)
        {
            throw new NotImplementedException();
        }

        public static string GetEnumDescription<TEnum>(TEnum enumeration)
        {
            return string.Empty;
        }
    }

    public enum DateFilterType
    {
        Equals = 0,
        Before = 1,
        After = 2
    }

    public class FilterDateRange
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public DateTime? FromLocal
        {
            get { return From.HasValue ? (DateTime?)From.Value.ToLocalTime() : null; }
        }

        public DateTime? ToLocal
        {
            get { return To.HasValue ? (DateTime?)To.Value.ToLocalTime() : null; }
        }

        public DateFilterType Type { get; set; }

        public Expression<Func<DateTime?, bool>> GetFilterExpression()
        {
            if (From == null && To == null)
            {
                return (date) => true;
            }

            var fromLocal = FromLocal;
            var toLocal = ToLocal;

            if (fromLocal != null && toLocal != null)
            {
                var toLocalTomorrow = toLocal.Value.AddDays(1);
                return (date) => fromLocal <= date
                    && date < toLocalTomorrow;
            }

            switch (Type)
            {
                case DateFilterType.Equals:
                    return (date) => date == FromLocal;

                case DateFilterType.Before:
                    return (date) => date <= FromLocal;

                case DateFilterType.After:
                    return (date) => date >= FromLocal;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return To.HasValue
                ? Type.GetDescription() + " From: " + FromLocal + " To: " + ToLocal
                : Type.GetDescription() + " From: " + FromLocal;
        }
    }
}