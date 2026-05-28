namespace FluentData.Core
{
    /// <summary>
    /// Provides helper methods for working with ADO.NET data readers.
    /// </summary>
    internal class DataReaderHelper
    {
        /// <summary>
        /// Extracts metadata from all columns in a data reader.
        /// </summary>
        /// <param name="reader">The data reader to extract column information from.</param>
        /// <returns>A list of <see cref="DataReaderField"/> objects containing column metadata. Duplicate column names (case-insensitive) are excluded.</returns>
        internal static List<DataReaderField> GetDataReaderFields(System.Data.IDataReader reader)
        {
            var columns = new List<DataReaderField>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var column = new DataReaderField(i, reader.GetName(i), reader.GetFieldType(i));

                if (columns.SingleOrDefault(x => x.LowerName == column.LowerName) == null)
                    columns.Add(column);
            }

            return columns;
        }
    }
}
