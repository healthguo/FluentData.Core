using System;
using System.Collections.Generic;
using System.Data;

namespace FluentData
{
    /// <summary>
    /// Provides a fluent interface for building and executing SELECT commands.
    /// Supports query construction, parameterization, projection and result mapping.
    /// </summary>
    /// <remarks>
    /// Use the fluent methods to assemble SQL clauses then call query methods (inherited from <see cref="ISelectBuilderAsync{TEntity}"/>)
    /// to execute and map results. Methods are chainable to produce readable query building code.
    /// </remarks>
    /// <typeparam name="TEntity">The entity type to map query results to.</typeparam>
    public interface ISelectBuilder<TEntity> : ISelectBuilderAsync<TEntity>
    {
        /// <summary>
        /// Gets or sets the select builder data containing SQL clauses and parameters.
        /// </summary>
        SelectBuilderData Data { get; set; }

        /// <summary>
        /// Specifies the SELECT clause columns.
        /// </summary>
        /// <param name="sql">The column names or expressions (e.g., "Id, Name, CreatedAt"). </param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> Select(string sql);

        /// <summary>
        /// Specifies the FROM clause table name.
        /// </summary>
        /// <param name="sql">The table name (e.g., "Users" or "Users u").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> From(string sql);

        /// <summary>
        /// Adds a WHERE clause condition.
        /// </summary>
        /// <param name="sql">The WHERE condition SQL (e.g., "Id = @Id").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> Where(string sql);

        /// <summary>
        /// Conditionally adds a WHERE clause condition.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="sql">The WHERE condition SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> WhereIf(bool condition, string sql);

        /// <summary>
        /// Adds an AND WHERE clause condition.
        /// </summary>
        /// <param name="sql">The AND WHERE condition SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> AndWhere(string sql);

        /// <summary>
        /// Conditionally adds an AND WHERE clause condition.
        /// </summary>
        /// <param name="condition">If true, the AND WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="sql">The AND WHERE condition SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> AndWhereIf(bool condition, string sql);

        /// <summary>
        /// Adds an OR WHERE clause condition.
        /// </summary>
        /// <param name="sql">The OR WHERE condition SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> OrWhere(string sql);

        /// <summary>
        /// Conditionally adds an OR WHERE clause condition.
        /// </summary>
        /// <param name="condition">If true, the OR WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="sql">The OR WHERE condition SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> OrWhereIf(bool condition, string sql);

        /// <summary>
        /// Adds a GROUP BY clause.
        /// </summary>
        /// <param name="sql">The GROUP BY columns (e.g., "CategoryId").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> GroupBy(string sql);

        /// <summary>
        /// Adds an ORDER BY clause.
        /// </summary>
        /// <param name="sql">The ORDER BY columns (e.g., "Name ASC" or "CreatedAt DESC").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> OrderBy(string sql);

        /// <summary>
        /// Conditionally adds an ORDER BY clause.
        /// </summary>
        /// <param name="condition">If true, the ORDER BY clause is added; otherwise, it is skipped.</param>
        /// <param name="sql">The ORDER BY columns.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> OrderByIf(bool condition, string sql);

        /// <summary>
        /// Adds a HAVING clause for filtering grouped results.
        /// </summary>
        /// <param name="sql">The HAVING condition SQL (e.g., "COUNT(*) > 1").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> Having(string sql);

        /// <summary>
        /// Configures paging for the SELECT command.
        /// </summary>
        /// <param name="currentPage">The current page number (1-based).</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage);

        /// <summary>
        /// Adds a parameter to the SELECT command.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="direction">The parameter direction. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0);

        /// <summary>
        /// Adds multiple positional parameters to the SELECT command.
        /// </summary>
        /// <param name="parameters">The parameter values to add.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        ISelectBuilder<TEntity> Parameters(params object[] parameters);

        /// <summary>
        /// Executes the SELECT command and returns a list of entities.
        /// </summary>
        /// <param name="customMapper">Optional custom mapping action to populate entity properties from the data reader.</param>
        /// <returns>A list of mapped entities.</returns>
        List<TEntity> QueryMany(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Executes the SELECT command and returns a list of entities using dynamic mapping.
        /// </summary>
        /// <param name="customMapper">A custom mapping action to populate entity properties from a dynamic object.</param>
        /// <returns>A list of mapped entities.</returns>
        List<TEntity> QueryMany(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Executes the SELECT command and returns a list of entities in a specific collection type.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return (must implement <see cref="IList{TEntity}"/>).</typeparam>
        /// <param name="customMapper">Optional custom mapping action to populate entity properties from the data reader.</param>
        /// <returns>The mapped entities in the specified collection type.</returns>
        TList QueryMany<TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;

        /// <summary>
        /// Executes the SELECT command and returns a list of entities in a specific collection type using dynamic mapping.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return (must implement <see cref="IList{TEntity}"/>).</typeparam>
        /// <param name="customMapper">A custom mapping action to populate entity properties from a dynamic object.</param>
        /// <returns>The mapped entities in the specified collection type.</returns>
        TList QueryMany<TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>;

        /// <summary>
        /// Executes the SELECT command and populates an existing list with entities.
        /// </summary>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action that receives the list and data reader for each row.</param>
        void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);

        /// <summary>
        /// Executes the SELECT command and populates an existing list with entities using dynamic mapping.
        /// </summary>
        /// <param name="list">The list to populate with query results.</param>
        /// <param name="customMapper">A custom mapping action that receives the list and dynamic object for each row.</param>
        void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper);

        /// <summary>
        /// Executes the SELECT command and returns a single entity. Throws if no result is found.
        /// </summary>
        /// <param name="customMapper">Optional custom mapping action to populate entity properties from the data reader.</param>
        /// <returns>The mapped entity.</returns>
        TEntity QuerySingle(Action<TEntity, IDataReader> customMapper = null);

        /// <summary>
        /// Executes the SELECT command and returns a single entity using dynamic mapping. Throws if no result is found.
        /// </summary>
        /// <param name="customMapper">A custom mapping action to populate entity properties from a dynamic object.</param>
        /// <returns>The mapped entity.</returns>
        TEntity QuerySingle(Action<TEntity, dynamic> customMapper);

        /// <summary>
        /// Executes the SELECT command with a custom mapper function for complex mapping scenarios.
        /// </summary>
        /// <param name="customMapper">A function that receives the data reader and returns a mapped entity.</param>
        /// <returns>The mapped entity.</returns>
        TEntity QueryComplexSingle(Func<IDataReader, TEntity> customMapper);

        /// <summary>
        /// Executes the SELECT command with a custom mapper function using dynamic mapping.
        /// </summary>
        /// <param name="customMapper">A function that receives a dynamic object and returns a mapped entity.</param>
        /// <returns>The mapped entity.</returns>
        TEntity QueryComplexSingle(Func<dynamic, TEntity> customMapper);

        /// <summary>
        /// Executes the SELECT command and returns the result as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the query result.</returns>
        /// <exception cref="FluentDataException">If query execution or result loading fails.</exception>
        DataTable QueryDataTable();
    }
}
