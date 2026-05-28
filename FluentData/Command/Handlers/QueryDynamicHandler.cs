using System;

namespace FluentData
{
    /// <summary>
    /// Handles query result mapping for dynamic entities using <see cref="ExpandoObject"/>.
    /// Automatically maps data reader columns to dynamic object properties.
    /// </summary>
    /// <typeparam name="TEntity">The entity type (must be dynamic/ExpandoObject).</typeparam>
    internal class QueryDynamicHandler<TEntity> : IQueryTypeHandler<TEntity>
    {
        private readonly DbCommandData _data;

        private readonly DynamicTypeAutoMapper _autoMapper;

        /// <summary>
        /// Gets a value indicating whether to iterate through the data reader row by row.
        /// Always returns true for dynamic handlers.
        /// </summary>
        public bool IterateDataReader { get { return true; } }

        /// <summary>
        /// Creates a new instance of <see cref="QueryDynamicHandler{TEntity}"/>.
        /// </summary>
        /// <param name="data">The command data containing execution context.</param>
        public QueryDynamicHandler(DbCommandData data)
        {
            _data = data;
            _autoMapper = new DynamicTypeAutoMapper(_data.Reader.InnerReader);
        }

        /// <summary>
        /// Maps the current data reader row to a dynamic entity object.
        /// </summary>
        /// <param name="customMapperReader">Unused for dynamic handlers.</param>
        /// <param name="customMapperDynamic">Unused for dynamic handlers.</param>
        /// <returns>A dynamic entity object with mapped properties.</returns>
        public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
        {
            var item = _autoMapper.AutoMap();
            return item;
        }
    }
}
