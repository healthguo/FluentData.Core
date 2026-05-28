using System;
using System.Collections.Generic;

namespace FluentData.Providers.Common
{
    /// <summary>
    /// Maps CLR types to corresponding database data types.
    /// Uses a static dictionary for efficient type lookup with thread-safe initialization.
    /// </summary>
    internal class DbTypeMapper
    {
        /// <summary>
        /// Static dictionary mapping CLR types to database data types.
        /// </summary>
        private static Dictionary<Type, DataTypes> _types;

        /// <summary>
        /// Lock object for thread-safe dictionary initialization.
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// Maps a CLR type to the corresponding database data type.
        /// Returns <see cref="DataTypes.Object"/> for unmapped types.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            if (_types == null)
            {
                lock (_locker)
                {
                    if (_types == null)
                    {
                        _types = new Dictionary<Type, DataTypes>
                        {
                            { typeof(short), DataTypes.Int16 },
                            { typeof(int), DataTypes.Int32 },
                            { typeof(long), DataTypes.Int64 },
                            { typeof(string), DataTypes.String },
                            { typeof(DateTime), DataTypes.DateTime },
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
            }

            if (!_types.ContainsKey(clrType))
                return DataTypes.Object;

            var dbType = _types[clrType];
            return dbType;
        }
    }
}
