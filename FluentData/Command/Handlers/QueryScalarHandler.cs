using System;

namespace FluentData
{
    /// <summary>
    /// Handles query result mapping for scalar/primitive types.
    /// Converts database values to the target type using type conversion.
    /// </summary>
    /// <typeparam name="TEntity">The scalar type to convert values to.</typeparam>
    internal class QueryScalarHandler<TEntity> : IQueryTypeHandler<TEntity>
    {
        private readonly DbCommandData _data;

        private Type _fieldType;

        /// <summary>
        /// Gets a value indicating whether to iterate through the data reader row by row.
        /// Always returns true for scalar handlers.
        /// </summary>
        public bool IterateDataReader { get { return true; } }

        /// <summary>
        /// Creates a new instance of <see cref="QueryScalarHandler{TEntity}"/>.
        /// </summary>
        /// <param name="data">The command data containing execution context.</param>
        public QueryScalarHandler(DbCommandData data)
        {
            _data = data;
        }

        /// <summary>
        /// Reads the first column value from the current row and converts it to the target type.
        /// </summary>
        /// <param name="customMapperReader">Unused for scalar handlers.</param>
        /// <param name="customMapperDynamic">Unused for scalar handlers.</param>
        /// <returns>The converted scalar value, or default if null.</returns>
        public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
        {
            var value = _data.Reader.GetValue(0);
            if (_fieldType == null)
                _fieldType = _data.Reader.GetFieldType(0);

            if (value == null)
                value = default(TEntity);
            else if (_fieldType != typeof(TEntity))
                value = (Convert.ChangeType(value, typeof(TEntity)));
            return (TEntity)value;
        }
    }
}
