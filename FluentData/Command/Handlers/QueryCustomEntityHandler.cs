using System;

namespace FluentData
{
    /// <summary>
    /// Handles query result mapping for custom entity types using auto-mapping.
    /// Supports custom mapper actions for manual property mapping.
    /// </summary>
    /// <typeparam name="TEntity">The custom entity type to map results to.</typeparam>
    internal class QueryCustomEntityHandler<TEntity> : IQueryTypeHandler<TEntity>
    {
        private readonly AutoMapper _autoMapper;

        private readonly DbCommandData _data;

        /// <summary>
        /// Creates a new instance of <see cref="QueryCustomEntityHandler{TEntity}"/>.
        /// </summary>
        /// <param name="data">The command data containing execution context.</param>
        public QueryCustomEntityHandler(DbCommandData data)
        {
            _data = data;
            _autoMapper = new AutoMapper(_data, typeof(TEntity));
        }

        /// <summary>
        /// Gets a value indicating whether to iterate through the data reader row by row.
        /// Always returns true for custom entity handlers.
        /// </summary>
        public bool IterateDataReader { get { return true; } }

        /// <summary>
        /// Maps the current data reader row to a custom entity object.
        /// Uses custom mapper if provided, otherwise uses auto-mapping.
        /// </summary>
        /// <param name="customMapperReader">An optional custom mapper action using <see cref="IDataReader"/>.</param>
        /// <param name="customMapperDynamic">An optional custom mapper action using dynamic object.</param>
        /// <returns>The mapped custom entity object.</returns>
        public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
        {
            var item = (TEntity)_data.Context.Data.EntityFactory.Create(typeof(TEntity));

            if (customMapperReader != null)
                customMapperReader(item, _data.Reader);
            else if (customMapperDynamic != null)
                customMapperDynamic(item, new DynamicDataReader(_data.Reader.InnerReader));
            else
                _autoMapper.AutoMap(item);
            return item;
        }
    }
}
