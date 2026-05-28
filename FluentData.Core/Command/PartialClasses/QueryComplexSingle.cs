using System.Data;

namespace FluentData.Core
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Executes the query with a complex mapper function and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps <see cref="IDataReader"/> to entity.</param>
        /// <returns>A single entity, or default if no results.</returns>
        public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                if (Data.Reader.Read())
                    item = customMapper(Data.Reader);
            }, typeof(TEntity) != typeof(DataTable));

            return item;
        }

        /// <summary>
        /// Executes the query with a complex mapper function using dynamic object and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps dynamic object to entity.</param>
        /// <returns>A single entity, or default if no results.</returns>
        public TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                if (Data.Reader.Read())
                    item = customMapper(new DynamicDataReader(Data.Reader));
            }, typeof(TEntity) != typeof(DataTable));

            return item;
        }

        /// <summary>
        /// Executes the query asynchronously with a complex mapper function and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps <see cref="IDataReader"/> to entity.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QueryComplexSingleAsync<TEntity>(Func<IDataReader, TEntity> customMapper)
        {
            return Task.FromResult(QueryComplexSingle(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously with a complex mapper function using dynamic object and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps dynamic object to entity.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QueryComplexSingleAsync<TEntity>(Func<dynamic, TEntity> customMapper)
        {
            return Task.FromResult(QueryComplexSingle(customMapper));
        }

    }
}
