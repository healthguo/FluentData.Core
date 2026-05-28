using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Provides synchronous query execution and result mapping capability.
    /// </summary>
    /// <remarks>
    /// Methods in this interface execute the command represented by the enclosing builder/command
    /// and map rows to CLR types. For complex mappings, pass a <c>customMapper</c> to control
    /// how columns are converted to entity properties.
    /// </remarks>
    public interface IQuery
    {
        /// <summary>
        /// Executes the query and returns a list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapping action to map data reader columns to entity properties.
        /// If provided this action will be invoked for each row. If null, the library attempts automatic mapping.</param>
        /// <returns>A list of entities of type <typeparamref name="TEntity"/>. Returns an empty list if no rows are returned.</returns>
        /// <exception cref="FluentDataException">If mapping fails and auto-mapping is not configured to ignore failures.</exception>
        List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Executes the query and returns a list of entities using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A list of entities of type <typeparamref name="TEntity"/>.</returns>
        List<TEntity> QueryMany<TEntity>(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Executes the query and returns results in a custom collection type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of collection to return, must implement <see cref="IList{TEntity}"/>.</typeparam>
        /// <param name="customMapper">An optional custom mapping action to map data reader columns to entity properties.</param>
        /// <returns>A collection of entities of type <typeparamref name="TList"/>.</returns>
        TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;

        /// <summary>
        /// Executes the query and returns results in a custom collection type using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of collection to return, must implement <see cref="IList{TEntity}"/>.</typeparam>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A collection of entities of type <typeparamref name="TList"/>.</returns>
        TList QueryMany<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>;

        /// <summary>
        /// Executes the query and populates a provided list with the results.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action to map data reader columns to entity properties.</param>
        void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);

        /// <summary>
        /// Executes the query and populates a provided list with the results using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper);

        /// <summary>
        /// Executes the query and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapping action to map data reader columns to entity properties.</param>
        /// <returns>A single entity of type <typeparamref name="TEntity"/>, or default if no rows are returned.</returns>
        /// <exception cref="FluentDataException">If mapping fails and auto-mapping is not configured to ignore failures.</exception>
        TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Executes the query and returns a single entity using a dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapping action using dynamic object for column access.</param>
        /// <returns>A single entity of type <typeparamref name="TEntity"/>, or default if no rows are returned.</returns>
        TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Executes the query with a fully custom mapper function.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps the data reader to an entity.</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/>.</returns>
        TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper);

        /// <summary>
        /// Executes the query with a fully custom mapper function using dynamic object.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps the dynamic object to an entity.</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/>.</returns>
        TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper);

        /// <summary>
        /// Executes the query and returns the results as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the query results.</returns>
        /// <exception cref="FluentDataException">If query execution or result loading fails.</exception>
        DataTable QueryDataTable();
    }
}
