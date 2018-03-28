using Shop.Infrastructure.Repository;
using Shop.Infrastructure.TableCreator.ColumnFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shop.Infrastructure.TableCreator
{
    /// <summary>
    /// Base class
    /// </summary>
    /// <typeparam name="T">Type of the table represents</typeparam>
    public abstract class TableColumn<T> : ITableColumn<T> where T : IId
    {
        protected TableColumn(TableColumnType columnType, string columnIdentifierName, string title)
        {
            Title = title;
            Identifier = new TableColumnIdentifier(columnIdentifierName, columnType);
        }

        public string Title { get; private set; }

        public TableColumnIdentifier Identifier { get; private set; }

        public virtual bool IsSortDisable => false;

        public virtual bool IsFilterDisable => false;

        public abstract TableColumnFilterStrategy FilterStrategy
        {
            // This will be used in TPSR-1536 to describe the filter mechanism (and therefore the UI) used by the column.
            get;
        }

        public virtual IEnumerable<string> StringFilterValues => Enumerable.Empty<string>();

        public virtual int? HeadlineSequenceNumber => null;

        public abstract string GetValueAsString(T offer);

        /// <summary>
        /// When implemented in a derived class, applies a filter to a specified query using a criterion on the derived column type./// </summary>
        /// <param name="sourceQuery">Product offers list query</param>
        /// <param name="filter">Column filter parameter</param>
        /// <returns>A new query, based on sourceQuery, that applies a filter that might reduce the number of rows returned to those that match a criterion specified in the derived column type.</returns>
        public IQueryable<T> ApplyFilter(IQueryable<T> sourceQuery, TableColumnFilter filter)
        {
            switch (FilterStrategy)
            {
                case TableColumnFilterStrategy.FreeText:
                    if (filter.FilterFreeText == null)
                    {
                        throw new ArgumentException("Free text must have filter text");
                    }

                    break;

                case TableColumnFilterStrategy.MultiSelection:
                    if (filter.MultiselectText == null)
                    {
                        throw new ArgumentException("Multi selection must have filter text");
                    }

                    break;

                case TableColumnFilterStrategy.Flag:
                    if (filter.Boolean == null)
                    {
                        throw new ArgumentException("Flag must have filter boolean");
                    }

                    break;

                case TableColumnFilterStrategy.NumericRange:
                    if (filter.NumericRange == null)
                    {
                        throw new ArgumentException("Numeric Range must have numeric filter");
                    }

                    break;

                case TableColumnFilterStrategy.DateRange:
                    if (filter.FilterDateRange == null)
                    {
                        throw new ArgumentException("Date range must have DateRange filter");
                    }

                    break;

                default:
                    break;
            }

            return Filter(sourceQuery, filter);
        }

        /// <summary>
        /// When implemented in a derived class, applies a sort to a specified query using the derived column type.
        /// </summary>
        /// <param name="sourceQuery">Product offers list query</param>
        /// <param name="isAscending">Is it an ascending sort request</param>
        /// <returns>A new query, based on sourceQuery, that sorts the ProductOffers by the values in the OfferViewColumn and selects the ProductOffer IDs.</returns>
        public abstract IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending);

        protected abstract IQueryable<T> Filter(IQueryable<T> sourceQuery, TableColumnFilter filter);

        public TableColumnDefinition GetColumnDefinition()
        {
            var definition = new TableColumnDefinition();
            definition.Title = Title;
            definition.Identifier = Identifier;
            definition.FilterStrategy = FilterStrategy;
            definition.IsFilterDisable = IsFilterDisable;
            definition.IsSortDisable = IsSortDisable;
            definition.StringFilterValues = StringFilterValues.ToList();
            return definition;
        }
    }

    public abstract class StringTableColumn<T> : TableColumn<T> where T : IId
    {
        protected StringTableColumn(TableColumnType columnType, string columnIdentifierName, string title)
            : base(columnType, columnIdentifierName, title)
        {
        }

        public override TableColumnFilterStrategy FilterStrategy => TableColumnFilterStrategy.FreeText;

        public override string GetValueAsString(T entity)
        {
            try
            {
                return EntityLambdaExpression().Compile().Invoke(entity);
            }
            catch (Exception)
            {
                var typeName = entity.GetType().FullName;
                var lambdaAsString = EntityLambdaExpression().ToString();
                throw new Exception("Cannot invoke value as string. entity: " + typeName + ", lambda: " + lambdaAsString + ", id: " + entity.Id);
            }
        }

        public override IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending)
        {
            var orderingLambda = EntityLambdaExpression();
            return isAscending
                ? sourceQuery.OrderBy(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id)
                : sourceQuery.OrderByDescending(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id);
        }

        protected override IQueryable<T> Filter(IQueryable<T> sourceQuery, TableColumnFilter filter)
        {
            var filterExpression = filter.FilterFreeText.GetFilterExpression();
            var offerViewNameExpression = EntityLambdaExpression();
            return sourceQuery.Where(offerViewNameExpression.Chain(filterExpression));
        }

        protected abstract Expression<Func<T, string>> EntityLambdaExpression();
    }

    public abstract class DateTimeTableColumn<T> : TableColumn<T> where T : IId
    {
        ////protected const string DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";
        protected const string DateTimeFormat = "dd MMM, yyyy H:mm:ss z";

        protected DateTimeTableColumn(TableColumnType columnType, string columnIdentifierName, string title)
            : base(columnType, columnIdentifierName, title)
        {
        }

        public override TableColumnFilterStrategy FilterStrategy => TableColumnFilterStrategy.DateRange;

        public override string GetValueAsString(T entity)
        {
            try
            {
                var dateTimeValue = EntityLambdaExpression().Compile().Invoke(entity);
                return dateTimeValue.HasValue
                    ? dateTimeValue.Value.ToString(DateTimeFormat)
                    : string.Empty;
            }
            catch (Exception)
            {
                var typeName = entity.GetType().FullName;
                var lambdaAsString = EntityLambdaExpression().ToString();
                throw new Exception("Cannot invoke value as string. entity: " + typeName + ", lambda: " + lambdaAsString + ", id: " + entity.Id);
            }
        }

        public override IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending)
        {
            var orderingLambda = EntityLambdaExpression();
            return isAscending
                ? sourceQuery.OrderBy(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id)
                : sourceQuery.OrderByDescending(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id);
        }

        protected override IQueryable<T> Filter(IQueryable<T> sourceQuery, TableColumnFilter filter)
        {
            var filterExpression = filter.FilterDateRange.GetFilterExpression();
            var offerViewNameExpression = EntityLambdaExpression();
            return sourceQuery.Where(offerViewNameExpression.Chain(filterExpression));
        }

        protected abstract Expression<Func<T, DateTime?>> EntityLambdaExpression();
    }

    public abstract class EnumTableColumn<T, TEnum> : TableColumn<T>
        where TEnum : struct
        where T : IId
    {
        private readonly List<string> _stringValidValues;

        protected EnumTableColumn(TableColumnType columnType, string columnIdentifierName, string title)
            : base(columnType, columnIdentifierName, title)
        {
            _stringValidValues = EnumHelper.CreateEnumDescriptionDictionary<TEnum>().Select(e => e.Value).ToList();
        }

        public override IEnumerable<string> StringFilterValues => _stringValidValues;

        public override TableColumnFilterStrategy FilterStrategy => TableColumnFilterStrategy.MultiSelection;

        public override string GetValueAsString(T entity)
        {
            try
            {
                return EnumHelper.GetEnumDescription(EntityLambdaExpression().Compile().Invoke(entity) as Enum);
            }
            catch (Exception)
            {
                var typeName = entity.GetType().FullName;
                var lambdaAsString = EntityLambdaExpression().ToString();
                throw new Exception("Cannot invoke value as string. entity: " + typeName + ", lambda: " + lambdaAsString + ", id: " + entity.Id);
            }
        }

        public override IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending)
        {
            var orderingLambda = EntityLambdaExpression();
            return isAscending
                ? sourceQuery.OrderBy(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id)
                : sourceQuery.OrderByDescending(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id);
        }

        protected override IQueryable<T> Filter(IQueryable<T> sourceQuery, TableColumnFilter filter)
        {
            var filterEnumList = filter.MultiselectText.GetEnumsOfStringValues<TEnum>();
            var expr = EntityLambdaExpression();
            return sourceQuery.Where(expr.Chain(tenum => filterEnumList.Contains(tenum)));
        }

        protected abstract Expression<Func<T, TEnum>> EntityLambdaExpression();
    }

    public abstract class StringMultiselectTableColumn<T> : TableColumn<T> where T : IId
    {
        private readonly List<string> _stringValidValues;

        protected StringMultiselectTableColumn(TableColumnType columnType, string columnIdentifierName, string title, List<string> stringValidValues)
            : base(columnType, columnIdentifierName, title)
        {
            _stringValidValues = stringValidValues;
        }

        protected StringMultiselectTableColumn(TableColumnType columnType, string columnIdentifierName, string title)
            : base(columnType, columnIdentifierName, title)
        {
        }

        public override IEnumerable<string> StringFilterValues => _stringValidValues;

        public override TableColumnFilterStrategy FilterStrategy => TableColumnFilterStrategy.MultiSelection;

        public override string GetValueAsString(T entity)
        {
            try
            {
                return EntityLambdaExpression().Compile().Invoke(entity);
            }
            catch (Exception)
            {
                var typeName = entity.GetType().FullName;
                var lambdaAsString = EntityLambdaExpression().ToString();
                throw new Exception("Cannot invoke value as string. entity: " + typeName + ", lambda: " + lambdaAsString + ", id: " + entity.Id);
            }
        }

        public override IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending)
        {
            var orderingLambda = EntityLambdaExpression();
            return isAscending
                ? sourceQuery.OrderBy(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id)
                : sourceQuery.OrderByDescending(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id);
        }

        protected override IQueryable<T> Filter(IQueryable<T> sourceQuery, TableColumnFilter filter)
        {
            var filterEnumList = filter.MultiselectText.StringValues;
            var expr = EntityLambdaExpression();
            return sourceQuery.Where(expr.Chain(tenum => filterEnumList.Contains(tenum)));
        }

        protected abstract Expression<Func<T, string>> EntityLambdaExpression();
    }

    public abstract class FlagTableColumn<T> : TableColumn<T> where T : IId
    {
        protected FlagTableColumn(TableColumnType columnType, string columnIdentifierName, string title)
            : base(columnType, columnIdentifierName, title)
        {
        }

        public override TableColumnFilterStrategy FilterStrategy => TableColumnFilterStrategy.Flag;

        public override string GetValueAsString(T entity)
        {
            try
            {
                return EntityLambdaExpression().Compile().Invoke(entity).ToString();
            }
            catch (Exception)
            {
                var typeName = entity.GetType().FullName;
                var lambdaAsString = EntityLambdaExpression().ToString();
                throw new Exception("Cannot invoke value as string. entity: " + typeName + ", lambda: " + lambdaAsString + ", id: " + entity.Id);
            }
        }

        public override IQueryable<int> ApplySort(IQueryable<T> sourceQuery, bool isAscending)
        {
            var orderingLambda = EntityLambdaExpression();
            return isAscending
                ? sourceQuery.OrderBy(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id)
                : sourceQuery.OrderByDescending(orderingLambda).ThenBy(po => po.Id).Select(po => po.Id);
        }

        protected override IQueryable<T> Filter(IQueryable<T> sourceQuery, TableColumnFilter filter)
        {
            var filterExpression = filter.Boolean.GetFilterExpression();
            var offerViewNameExpression = EntityLambdaExpression();
            return sourceQuery.Where(offerViewNameExpression.Chain(filterExpression));
        }

        protected abstract Expression<Func<T, bool>> EntityLambdaExpression();
    }
}