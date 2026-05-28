using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluentData
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Executes the query and populates an existing list using a custom mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action to map data reader to the list.</param>
        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                while (Data.Reader.Read())
                    customMapper(list, Data.Reader);
            }, typeof(TEntity) != typeof(DataTable));
        }

        /// <summary>
        /// Executes the query and populates an existing list using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                while (Data.Reader.Read())
                    customMapper(list, Data.Reader);
            }, typeof(TEntity) != typeof(DataTable));
        }

        /// <summary>
        /// Executes the query asynchronously and populates an existing list using a custom mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action to map data reader to the list.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task QueryComplexManyAsync<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            QueryComplexMany(list, customMapper);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the query asynchronously and populates an existing list using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task QueryComplexManyAsync<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            QueryComplexMany(list, customMapper);
            return Task.CompletedTask;
        }

    }
}
