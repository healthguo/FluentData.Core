using System.Data;
using System.Dynamic;

namespace FluentData.Core
{
    /// <summary>
    /// Orchestrates query execution by selecting the appropriate type handler based on the entity type.
    /// Routes to dynamic, DataTable, custom entity, or scalar handlers as needed.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to map query results to.</typeparam>
    internal class QueryHandler<TEntity>
    {
        private readonly DbCommandData _data;

        private readonly IQueryTypeHandler<TEntity> _typeHandler;

        /// <summary>
        /// Creates a new instance of <see cref="QueryHandler{TEntity}"/> and selects the appropriate type handler.
        /// </summary>
        /// <param name="data">The command data containing execution context.</param>
        public QueryHandler(DbCommandData data)
        {
            _data = data;
            if (typeof(TEntity) == typeof(object) || typeof(TEntity) == typeof(ExpandoObject))
                _typeHandler = new QueryDynamicHandler<TEntity>(data);
            else if (typeof(TEntity) == typeof(DataTable))
                _typeHandler = new QueryDataTableHandler<TEntity>(data);
            else if (ReflectionHelper.IsCustomEntity<TEntity>())
                _typeHandler = new QueryCustomEntityHandler<TEntity>(data);
            else
                _typeHandler = new QueryScalarHandler<TEntity>(data);
        }

        /// <summary>
        /// Executes the query and returns multiple entities in a custom list implementation.
        /// </summary>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapperReader">An optional custom mapper action using <see cref="IDataReader"/>.</param>
        /// <param name="customMapperDynamic">An optional custom mapper action using dynamic object.</param>
        /// <returns>A list of entities.</returns>
        internal TList ExecuteMany<TList>(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
            where TList : IList<TEntity>
        {
            var items = (TList)_data.Context.Data.EntityFactory.Create(typeof(TList));
            var reader = _data.Reader.InnerReader;

            if (_typeHandler.IterateDataReader)
            {
                while (reader.Read())
                {
                    var item = (TEntity)_typeHandler.HandleType(customMapperReader, customMapperDynamic);
                    items.Add(item);
                }
            }
            else
            {
                var item = (TEntity)_typeHandler.HandleType(customMapperReader, customMapperDynamic);
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Executes the query and returns a single entity.
        /// </summary>
        /// <param name="customMapperReader">An optional custom mapper action using <see cref="IDataReader"/>.</param>
        /// <param name="customMapperDynamic">An optional custom mapper action using dynamic object.</param>
        /// <returns>A single entity, or default if no results.</returns>
        internal TEntity ExecuteSingle(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
        {
            var item = default(TEntity);
            if (!_typeHandler.IterateDataReader || _data.Reader.InnerReader.Read())
                item = (TEntity)_typeHandler.HandleType(customMapperReader, customMapperDynamic);

            return item!;
        }
    }
}
