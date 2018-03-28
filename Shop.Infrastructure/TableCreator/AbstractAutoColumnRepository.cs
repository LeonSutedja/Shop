using Shop.Infrastructure.Repository;

namespace Shop.Infrastructure.TableCreator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    public interface ITableColumnRepository<T>
    {
        IList<ITableColumn<T>> GetAllViewColumns();

        ITableColumn<T> GetColumn(TableColumnIdentifier identifier);
    }

    public abstract class AbstractAutoColumnRepository<T> : ITableColumnRepository<T> where T : IId
    {
        private readonly IEnumerable<ITableColumn<T>> _propertyColumns;

        protected AbstractAutoColumnRepository(IEnumerable<ITableColumn<T>> injectedTableColumns)
        {
            var columnList = new List<ITableColumn<T>>();
            var characteristicDefinitionType = typeof(T);
            var listInjectedName = injectedTableColumns.Select(col => col.Identifier.AdditionalData).ToList();
            
            var properties = characteristicDefinitionType.GetProperties();

            // exclude properties
            properties = properties.Where(p => !ExcludeProperties().Contains(p.Name)).ToArray();

            foreach (var property in properties)
            {
                var param = Expression.Parameter(characteristicDefinitionType);
                var field = Expression.PropertyOrField(param, property.Name);
                var title = Regex.Replace(property.Name, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");

                if (!property.CanRead || !property.CanWrite || listInjectedName.Contains(property.Name))
                {
                    continue;
                }

                if (property.PropertyType == typeof(string))
                {
                    var expression = Expression.Lambda<Func<T, string>>(field, param);
                    var newColumn = new PublicStringTableColumn(TableColumnType.Property, property.Name, title, expression);

                    columnList.Add(newColumn);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    var expression = Expression.Lambda<Func<T, bool>>(field, param);
                    var newColumn = new PublicFlagTableColumn(TableColumnType.Property, property.Name, title, expression);
                    columnList.Add(newColumn);
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    var expression = Expression.Lambda<Func<T, DateTime?>>(field, param);
                    var newColumn = new PublicDateTimeTableColumn(TableColumnType.Property, property.Name, title, expression);
                    columnList.Add(newColumn);
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    var expressionDateTime = Expression.Lambda<Func<T, DateTime>>(field, param);
                    Expression converted = Expression.Convert(expressionDateTime.Body, typeof(DateTime?));
                    var expressionNullableDateTime = Expression.Lambda<Func<T, DateTime?>>(converted, expressionDateTime.Parameters);
                    var newColumn = new PublicDateTimeTableColumn(TableColumnType.Property, property.Name, title, expressionNullableDateTime);
                    columnList.Add(newColumn);
                }
            }

            var allPropertyColumns = injectedTableColumns.Where(col => col.Identifier.ColumnType == TableColumnType.Property && col.Title != "EXCLUDE").ToList();
            allPropertyColumns.AddRange(columnList);
            _propertyColumns = allPropertyColumns;
        }
        
        public IList<ITableColumn<T>> GetAllViewColumns()
        {
            return _propertyColumns.ToList();
        }
        
        public ITableColumn<T> GetColumn(TableColumnIdentifier identifier)
        {
            return _propertyColumns.First(col => col.Identifier.Equals(identifier));
        }

        protected virtual List<string> ExcludeProperties()
        {
            return new List<string>();
        }

        private class PublicFlagTableColumn : FlagTableColumn<T>
        {
            private readonly Expression<Func<T, bool>> _lambdaExpression;

            public PublicFlagTableColumn(
                TableColumnType columnType,
                string columnIdentifierName,
                string title,
                Expression<Func<T, bool>> lambdaExpression)
                : base(columnType, columnIdentifierName, title)
            {
                _lambdaExpression = lambdaExpression;
            }

            protected override Expression<Func<T, bool>> EntityLambdaExpression()
            {
                return _lambdaExpression;
            }
        }

        private class PublicDateTimeTableColumn : DateTimeTableColumn<T>
        {
            private readonly Expression<Func<T, DateTime?>> _lambdaExpression;

            public PublicDateTimeTableColumn(
                TableColumnType columnType,
                string columnIdentifierName,
                string title,
                Expression<Func<T, DateTime?>> lambdaExpression)
                : base(columnType, columnIdentifierName, title)
            {
                _lambdaExpression = lambdaExpression;
            }

            protected override Expression<Func<T, DateTime?>> EntityLambdaExpression()
            {
                return _lambdaExpression;
            }
        }

        private class PublicStringTableColumn : StringTableColumn<T>
        {
            private readonly Expression<Func<T, string>> _lambdaExpression;

            public PublicStringTableColumn(
                TableColumnType columnType,
                string columnIdentifierName,
                string title,
                Expression<Func<T, string>> lambdaExpression)
                : base(columnType, columnIdentifierName, title)
            {
                _lambdaExpression = lambdaExpression;
            }

            protected override Expression<Func<T, string>> EntityLambdaExpression()
            {
                return _lambdaExpression;
            }
        }
    }
}
