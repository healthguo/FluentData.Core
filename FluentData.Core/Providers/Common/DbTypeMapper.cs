using System.Xml.Linq;

namespace FluentData.Core.Providers.Common
{
    /// <summary>
    /// Maps CLR types to FluentData database data types.
    /// Uses a cached dictionary for performance with thread-safe initialization.
    /// </summary>
    internal class DbTypeMapper
    {
        private static Dictionary<Type, DataTypes>? _types;

        private static readonly object _locker = new();

        /// <summary>
        /// Gets the FluentData database type for a given CLR type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value, or <see cref="DataTypes.Object"/> if no mapping exists.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            if (_types == null)
            {
                lock (_locker)
                {
                    _types = new Dictionary<Type, DataTypes>
                    {
                        { typeof(short), DataTypes.Int16 },
                        { typeof(int), DataTypes.Int32 },
                        { typeof(long), DataTypes.Int64 },
                        { typeof(string), DataTypes.String },
                        { typeof(DateTime), DataTypes.DateTime },
                        { typeof(XDocument), DataTypes.Xml },
                        { typeof(decimal), DataTypes.Decimal },
                        { typeof(Guid), DataTypes.Guid },
                        { typeof(bool), DataTypes.Boolean },
                        { typeof(char), DataTypes.String },
                        { typeof(DBNull), DataTypes.String },
                        { typeof(float), DataTypes.Single },
                        { typeof(double), DataTypes.Double },
                        { typeof(byte[]), DataTypes.Binary }
                    };
                }
            }

            if (!_types.ContainsKey(clrType))
                return DataTypes.Object;

            return _types[clrType];
        }
    }
}
