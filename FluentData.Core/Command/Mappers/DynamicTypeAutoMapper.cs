using System.Dynamic;

namespace FluentData.Core
{
    /// <summary>
    /// Automatically maps data reader fields to a dynamic <see cref="ExpandoObject"/>.
    /// Creates dynamic objects with properties matching data reader columns.
    /// </summary>
    internal class DynamicTypeAutoMapper
    {
        private readonly List<DataReaderField> _fields;

        private readonly System.Data.IDataReader _reader;

        /// <summary>
        /// Creates a new instance of <see cref="DynamicTypeAutoMapper"/>.
        /// </summary>
        /// <param name="reader">The data reader to map from.</param>
        public DynamicTypeAutoMapper(System.Data.IDataReader reader)
        {
            _reader = reader;
            _fields = DataReaderHelper.GetDataReaderFields(_reader);
        }

        /// <summary>
        /// Maps the current data reader row to a dynamic <see cref="ExpandoObject"/>.
        /// </summary>
        /// <returns>A dynamic object with properties matching the data reader columns.</returns>
        public ExpandoObject AutoMap()
        {
            var item = new ExpandoObject();

            var itemDictionary = item as IDictionary<string, object?>;

            foreach (var column in _fields)
            {
                if (_reader.IsDBNull(column.Index))
                    itemDictionary.Add(column.Name, null);
                else
                    itemDictionary.Add(column.Name, _reader[column.Index]);
            }

            return item;
        }
    }
}
