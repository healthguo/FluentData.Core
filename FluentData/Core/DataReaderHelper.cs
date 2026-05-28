using System.Collections.Generic;
using System.Linq;

namespace FluentData
{
    /// <summary>
    /// Provides helper methods for working with IDataReader results.
    /// </summary>
    internal class DataReaderHelper
    {
        /// <summary>
        /// Extracts field metadata from an IDataReader, deduplicating by column name.
        /// </summary>
        /// <param name="reader">The data reader to extract fields from.</param>
        /// <returns>A list of DataReaderField objects containing field metadata.</returns>
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
