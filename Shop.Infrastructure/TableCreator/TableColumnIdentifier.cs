using Shop.Infrastructure.TableCreator.ColumnFilter;

namespace Shop.Infrastructure.TableCreator
{
    /// <summary>
    /// Provides identification for the column that is being shown / requested.
    /// </summary>
    public class TableColumnIdentifier
    {
        public TableColumnIdentifier(string additionalData, TableColumnType columnType)
        {
            AdditionalData = additionalData;
            ColumnType = columnType;
        }

        //// This class is also act as a dto
        public TableColumnType ColumnType { get; private set; }

        public string AdditionalData { get; private set; }

        public override bool Equals(object obj)
        {
            var idObj = obj as TableColumnIdentifier;
            if (idObj == null)
            {
                return false;
            }

            return (idObj.ColumnType == ColumnType) && (idObj.AdditionalData == AdditionalData);
        }

        public override int GetHashCode()
        {
            return (ColumnType.ToString() + AdditionalData).GetHashCode();
        }

        public override string ToString()
        {
            return EnumHelper.GetEnumDescription(ColumnType) + ": " + AdditionalData;
        }
    }
}