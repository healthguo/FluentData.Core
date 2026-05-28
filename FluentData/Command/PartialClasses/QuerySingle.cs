using System;
using System.Data;
using System.Threading.Tasks;

namespace FluentData
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Executes the query and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A single entity, or default if no results.</returns>
        public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                item = new QueryHandler<TEntity>(Data).ExecuteSingle(customMapper, null);
            }, typeof(TEntity) != typeof(DataTable));

            return item;
        }

        /// <summary>
        /// Executes the query and returns a single entity using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A single entity, or default if no results.</returns>
        public TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                item = new QueryHandler<TEntity>(Data).ExecuteSingle(customMapper, null);
            }, typeof(TEntity) != typeof(DataTable));

            return item;
        }

        /// <summary>
        /// Executes the query asynchronously and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QuerySingleAsync<TEntity>(Action<TEntity, IDataReader> customMapper = null)
        {
            return Task.FromResult(QuerySingle(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a single entity using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QuerySingleAsync<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return Task.FromResult(QuerySingle(customMapper));
        }

    }
}
