using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Provides asynchronous query execution and result mapping capability.
    /// </summary>
    /// <remarks>
    /// Async variants mirror the synchronous <see cref="IQuery"/> methods and return Tasks.
    /// Use these methods to avoid blocking threads for I/O-bound database operations.
    /// </remarks>
    public interface IQueryAsync
    {
        /// <summary>
        /// Asynchronously executes the query and returns a list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapping action to map data reader columns to entity properties.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of entities of type <typeparamref name="TEntity"/>.
        /// The task result is never null; an empty list is returned when there are no rows.</returns>
        /// <exception cref="FluentDataException">If automatic mapping fails and cannot be ignored.</exception>
        Task<List<TEntity>> QueryManyAsync<TEntity>(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Asynchronously executes the query and returns a list of entities using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of entities of type <typeparamref name="TEntity"/>.</returns>
        Task<List<TEntity>> QueryManyAsync<TEntity>(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Asynchronously executes the query and returns results in a custom collection type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of collection to return, must implement <see cref="IList{TEntity}"/>.</typeparam>
        /// <param name="customMapper">An optional custom mapping action to map data reader columns to entity properties.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities of type <typeparamref name="TList"/>.</returns>
        Task<TList> QueryManyAsync<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;

        /// <summary>
        /// Asynchronously executes the query and returns results in a custom collection type using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of collection to return, must implement <see cref="IList{TEntity}"/>.</typeparam>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities of type <typeparamref name="TList"/>.</returns>
        Task<TList> QueryManyAsync<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>;

        /// <summary>
        /// Asynchronously executes the query and populates a provided list with the results.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action to map data reader columns to entity properties.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task QueryComplexManyAsync<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);

        /// <summary>
        /// Asynchronously executes the query and populates a provided list with the results using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task QueryComplexManyAsync<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper);

        /// <summary>
        /// Asynchronously executes the query and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapping action to map data reader columns to entity properties.</param>
        /// <returns>A task representing the asynchronous operation, containing a single entity of type <typeparamref name="TEntity"/>, or default if no rows are returned.</returns>
        Task<TEntity> QuerySingleAsync<TEntity>(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Asynchronously executes the query and returns a single entity using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A task representing the asynchronous operation, containing a single entity of type <typeparamref name="TEntity"/>, or default if no rows are returned.</returns>
        Task<TEntity> QuerySingleAsync<TEntity>(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Asynchronously executes the query with a fully custom mapper function.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps the data reader to an entity.</param>
        /// <returns>A task representing the asynchronous operation, containing an entity of type <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> QueryComplexSingleAsync<TEntity>(Func<IDataReader, TEntity> customMapper);

        /// <summary>
        /// Asynchronously executes the query with a fully custom mapper function using dynamic object.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps the dynamic object to an entity.</param>
        /// <returns>A task representing the asynchronous operation, containing an entity of type <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> QueryComplexSingleAsync<TEntity>(Func<dynamic, TEntity> customMapper);

        /// <summary>
        /// Asynchronously executes the query and returns the results as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="DataTable"/> with the query results.</returns>
        /// <exception cref="FluentDataException">If query execution or result loading fails.</exception>
        Task<DataTable> QueryDataTableAsync();
    }
}
