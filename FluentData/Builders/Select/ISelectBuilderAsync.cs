using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluentData
{
    /// <summary>
    /// Provides asynchronous query operations for SELECT command results.
    /// </summary>
    /// <remarks>
    /// These methods mirror the synchronous query APIs but return <see cref="Task"/>-wrapped results.
    /// Use async methods in I/O-bound scenarios to keep calling threads responsive.
    /// </remarks>
    /// <typeparam name="TEntity">The entity type to map query results to.</typeparam>
    public interface ISelectBuilderAsync<TEntity>
    {
        /// <summary>
        /// Asynchronously executes the query and returns a list of entities.
        /// </summary>
        /// <param name="customMapper">Optional custom mapping action to populate entity properties from the data reader.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of mapped entities.</returns>
        Task<List<TEntity>> QueryManyAsync(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Asynchronously executes the query and returns a list of entities using dynamic mapping.
        /// </summary>
        /// <param name="customMapper">A custom mapping action to populate entity properties from a dynamic object.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of mapped entities.</returns>
        Task<List<TEntity>> QueryManyAsync(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Asynchronously executes the query and returns a list of entities in a specific collection type.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return (must implement <see cref="IList{TEntity}"/>).</typeparam>
        /// <param name="customMapper">Optional custom mapping action to populate entity properties from the data reader.</param>
        /// <returns>A task representing the asynchronous operation, containing the mapped entities in the specified collection type.</returns>
        Task<TList> QueryManyAsync<TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;

        /// <summary>
        /// Asynchronously executes the query and returns a list of entities in a specific collection type using dynamic mapping.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return (must implement <see cref="IList{TEntity}"/>).</typeparam>
        /// <param name="customMapper">A custom mapping action to populate entity properties from a dynamic object.</param>
        /// <returns>A task representing the asynchronous operation, containing the mapped entities in the specified collection type.</returns>
        Task<TList> QueryManyAsync<TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>;

        /// <summary>
        /// Asynchronously executes the SELECT command and populates an existing list with entities.
        /// </summary>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action that receives the list and data reader for each row.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task QueryComplexManyAsync(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);

        /// <summary>
        /// Asynchronously executes the SELECT command and populates an existing list with entities using dynamic mapping.
        /// </summary>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action that receives the list and dynamic object for each row.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task QueryComplexManyAsync(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper);

        /// <summary>
        /// Asynchronously executes the SELECT command and returns a single entity. Throws if no result is found.
        /// </summary>
        /// <param name="customMapper">Optional custom mapping action to populate entity properties from the data reader.</param>
        /// <returns>A task representing the asynchronous operation, containing the mapped entity.</returns>
        Task<TEntity> QuerySingleAsync(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Asynchronously executes the SELECT command and returns a single entity using dynamic mapping. Throws if no result is found.
        /// </summary>
        /// <param name="customMapper">A custom mapping action to populate entity properties from a dynamic object.</param>
        /// <returns>A task representing the asynchronous operation, containing the mapped entity.</returns>
        Task<TEntity> QuerySingleAsync(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Asynchronously executes the SELECT command with a custom mapper function for complex mapping scenarios.
        /// </summary>
        /// <param name="customMapper">A function that receives the data reader and returns a mapped entity.</param>
        /// <returns>A task representing the asynchronous operation, containing the mapped entity.</returns>
        Task<TEntity> QueryComplexSingleAsync(Func<IDataReader, TEntity> customMapper);

        /// <summary>
        /// Asynchronously executes the SELECT command with a custom mapper function using dynamic mapping.
        /// </summary>
        /// <param name="customMapper">A function that receives a dynamic object and returns a mapped entity.</param>
        /// <returns>A task representing the asynchronous operation, containing the mapped entity.</returns>
        Task<TEntity> QueryComplexSingleAsync(Func<dynamic, TEntity> customMapper);

        /// <summary>
        /// Asynchronously executes the SELECT command and returns the result as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing the query result as a <see cref="DataTable"/>.</returns>
        /// <exception cref="FluentDataException">If query execution or result loading fails.</exception>
        Task<DataTable> QueryDataTableAsync();
    }
}
