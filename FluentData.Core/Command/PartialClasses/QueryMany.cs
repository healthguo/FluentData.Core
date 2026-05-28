using System.Data;

namespace FluentData.Core
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Executes the query and returns a custom list implementation of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A list of entities.</returns>
        public TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader>? customMapper = null) where TList : IList<TEntity>
        {
            var items = default(TList);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                items = new QueryHandler<TEntity>(Data).ExecuteMany<TList>(customMapper, null);
            }, typeof(TEntity) != typeof(DataTable));

            return items!;
        }

        /// <summary>
        /// Executes the query and returns a custom list implementation using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A list of entities.</returns>
        public TList QueryMany<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
        {
            var items = default(TList);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                items = new QueryHandler<TEntity>(Data).ExecuteMany<TList>(null, customMapper);
            }, typeof(TEntity) != typeof(DataTable));

            return items!;
        }

        /// <summary>
        /// Executes the query and returns a list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A list of entities.</returns>
        public List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader>? customMapper)
        {
            return QueryMany<TEntity, List<TEntity>>(customMapper);
        }

        /// <summary>
        /// Executes the query and returns a list of entities using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A list of entities.</returns>
        public List<TEntity> QueryMany<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return QueryMany<TEntity, List<TEntity>>(customMapper);
        }

        /// <summary>
        /// Executes the query and returns results as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the query results.</returns>
        public DataTable QueryDataTable()
        {
            var dataTable = new DataTable();

            Data.ExecuteQueryHandler.ExecuteQuery(true, () => dataTable.Load(Data.Reader.InnerReader, LoadOption.OverwriteChanges), false);

            return dataTable;
        }

        /// <summary>
        /// Executes the query asynchronously and returns a list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A task returning a list of entities.</returns>
        public Task<List<TEntity>> QueryManyAsync<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return Task.FromResult(QueryMany(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a list of entities using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task returning a list of entities.</returns>
        public Task<List<TEntity>> QueryManyAsync<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return Task.FromResult(QueryMany(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a custom list implementation.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A task returning a list of entities.</returns>
        public Task<TList> QueryManyAsync<TEntity, TList>(Action<TEntity, IDataReader>? customMapper = null) where TList : IList<TEntity>
        {
            return Task.FromResult(QueryMany<TEntity, TList>(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a custom list using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task returning a list of entities.</returns>
        public Task<TList> QueryManyAsync<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
        {
            return Task.FromResult(QueryMany<TEntity, TList>(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns results as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A task returning a <see cref="DataTable"/> containing the query results.</returns>
        public Task<DataTable> QueryDataTableAsync()
        {
            return Task.FromResult(QueryDataTable());
        }
    }
}
