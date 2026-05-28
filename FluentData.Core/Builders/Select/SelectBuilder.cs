using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Implementation of the SELECT builder for constructing and executing SELECT queries.
    /// Provides a fluent interface for building SQL SELECT statements with support for paging, ordering, and filtering.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to map query results to.</typeparam>
    internal class SelectBuilder<TEntity> : ISelectBuilder<TEntity>
    {
        /// <summary>
        /// Gets or sets the select builder data containing command and SQL components.
        /// </summary>
        public SelectBuilderData Data { get; set; }

        /// <summary>
        /// Gets the actions handler for managing parameters.
        /// </summary>
        protected ActionsHandler Actions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="SelectBuilder{TEntity}"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        public SelectBuilder(IDbCommand command)
        {
            Data = new SelectBuilderData(command, "");
            Actions = new ActionsHandler(Data);
        }

        /// <summary>
        /// Prepares the database command by generating the SELECT SQL.
        /// </summary>
        /// <returns>The prepared <see cref="IDbCommand"/> ready for execution.</returns>
        /// <exception cref="FluentDataException">Thrown if paging is used without ORDER BY.</exception>
        private IDbCommand GetPreparedDbCommand()
        {
            if (Data.PagingItemsPerPage > 0 && string.IsNullOrEmpty(Data.OrderBy))
                throw new FluentDataException("Order by must defined when using Paging.");

            Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForSelectBuilder(Data));
            return Data.Command;
        }

        /// <summary>
        /// Specifies the columns to select.
        /// </summary>
        /// <param name="sql">The SELECT clause SQL (e.g., "Id, Name" or "*").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Select(string sql)
        {
            Data.Select += sql;
            return this;
        }

        /// <summary>
        /// Specifies the FROM clause.
        /// </summary>
        /// <param name="sql">The FROM clause SQL (e.g., "Products p").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> From(string sql)
        {
            Data.From += sql;
            return this;
        }

        /// <summary>
        /// Adds a WHERE clause condition.
        /// </summary>
        /// <param name="sql">The WHERE clause SQL without the "WHERE" keyword.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Where(string sql)
        {
            Data.WhereSql += sql;
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="sql">The WHERE clause SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> WhereIf(bool condition, string sql)
        {
            return condition ? this.Where(sql) : this;
        }

        /// <summary>
        /// Adds an AND WHERE clause condition.
        /// </summary>
        /// <param name="sql">The WHERE clause SQL to append with AND.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> AndWhere(string sql)
        {
            if (Data.WhereSql.Length > 0)
                Data.WhereSql += " and ";
            Data.WhereSql += sql;
            return this;
        }

        /// <summary>
        /// Conditionally adds an AND WHERE clause condition.
        /// </summary>
        /// <param name="condition">If true, the AND WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="sql">The WHERE clause SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> AndWhereIf(bool condition, string sql)
        {
            return condition ? this.AndWhere(sql) : this;
        }

        /// <summary>
        /// Adds an OR WHERE clause condition.
        /// </summary>
        /// <param name="sql">The WHERE clause SQL to append with OR.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> OrWhere(string sql)
        {
            if (Data.WhereSql.Length > 0)
                Data.WhereSql += " or ";
            Data.WhereSql += sql;
            return this;
        }


        /// <summary>
        /// Conditionally adds an OR WHERE clause condition.
        /// </summary>
        /// <param name="condition">If true, the OR WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="sql">The WHERE clause SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> OrWhereIf(bool condition, string sql)
        {
            return condition ? this.OrWhere(sql) : this;
        }

        /// <summary>
        /// Specifies the ORDER BY clause.
        /// </summary>
        /// <param name="sql">The ORDER BY clause SQL (e.g., "Name ASC").</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> OrderBy(string sql)
        {
            Data.OrderBy += sql;
            return this;
        }

        /// <summary>
        /// Conditionally specifies the ORDER BY clause.
        /// </summary>
        /// <param name="condition">If true, the ORDER BY is added; otherwise, it is skipped.</param>
        /// <param name="sql">The ORDER BY clause SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> OrderByIf(bool condition, string sql)
        {
            return condition ? this.OrderBy(sql) : this;
        }

        /// <summary>
        /// Specifies the GROUP BY clause.
        /// </summary>
        /// <param name="sql">The GROUP BY clause SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> GroupBy(string sql)
        {
            Data.GroupBy += sql;
            return this;
        }

        /// <summary>
        /// Specifies the HAVING clause.
        /// </summary>
        /// <param name="sql">The HAVING clause SQL.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Having(string sql)
        {
            Data.Having += sql;
            return this;
        }

        /// <summary>
        /// Configures paging for the query.
        /// </summary>
        /// <param name="currentPage">The current page number (1-based).</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage)
        {
            Data.PagingCurrentPage = currentPage;
            Data.PagingItemsPerPage = itemsPerPage;
            return this;
        }

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="direction">The parameter direction.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction, int size)
        {
            Data.Command.Parameter(name, value, parameterType, direction, size);
            return this;
        }

        /// <summary>
        /// Adds multiple parameters using positional values (@0, @1, etc.).
        /// </summary>
        /// <param name="parameters">The parameter values.</param>
        /// <returns>The current <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Parameters(params object[] parameters)
        {
            Data.Command.Parameters(parameters);
            return this;
        }

        /// <summary>
        /// Executes the query and returns a list of entities.
        /// </summary>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A list of entities.</returns>
        public List<TEntity> QueryMany(Action<TEntity, IDataReader>? customMapper = null)
        {
            return GetPreparedDbCommand().QueryMany(customMapper);
        }

        /// <summary>
        /// Executes the query and returns a list of entities using dynamic mapper.
        /// </summary>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A list of entities.</returns>
        public List<TEntity> QueryMany(Action<TEntity, dynamic> customMapper)
        {
            return GetPreparedDbCommand().QueryMany(customMapper);
        }

        /// <summary>
        /// Executes the query and returns a list using a specific collection type.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return.</typeparam>
        /// <param name="customMapper">An optional custom mapper action.</param>
        /// <returns>A collection of entities.</returns>
        public TList QueryMany<TList>(Action<TEntity, IDataReader>? customMapper = null) where TList : IList<TEntity>
        {
            return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
        }

        /// <summary>
        /// Executes the query and returns a list using a specific collection type with dynamic mapper.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A collection of entities.</returns>
        public TList QueryMany<TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
        {
            return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
        }

        /// <summary>
        /// Executes the query and populates an existing list with a custom mapper.
        /// </summary>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using IDataReader.</param>
        public void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            GetPreparedDbCommand().QueryComplexMany(list, customMapper);
        }

        /// <summary>
        /// Executes the query and populates an existing list with a dynamic custom mapper.
        /// </summary>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        public void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            GetPreparedDbCommand().QueryComplexMany(list, customMapper);
        }

        /// <summary>
        /// Executes the query and returns a single entity.
        /// </summary>
        /// <param name="customMapper">An optional custom mapper action.</param>
        /// <returns>A single entity, or default(TEntity) if no results.</returns>
        public TEntity QuerySingle(Action<TEntity, IDataReader>? customMapper = null)
        {
            return GetPreparedDbCommand().QuerySingle(customMapper);
        }

        /// <summary>
        /// Executes the query and returns a single entity using dynamic mapper.
        /// </summary>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A single entity, or default(TEntity) if no results.</returns>
        public TEntity QuerySingle(Action<TEntity, dynamic> customMapper)
        {
            return GetPreparedDbCommand().QuerySingle(customMapper);
        }

        /// <summary>
        /// Executes the query with a complex custom mapper using IDataReader.
        /// </summary>
        /// <param name="customMapper">A function that maps IDataReader to entity.</param>
        /// <returns>A single entity.</returns>
        public TEntity QueryComplexSingle(Func<IDataReader, TEntity> customMapper)
        {
            return GetPreparedDbCommand().QueryComplexSingle(customMapper);
        }

        /// <summary>
        /// Executes the query with a complex custom mapper using dynamic object.
        /// </summary>
        /// <param name="customMapper">A function that maps dynamic object to entity.</param>
        /// <returns>A single entity.</returns>
        public TEntity QueryComplexSingle(Func<dynamic, TEntity> customMapper)
        {
            return GetPreparedDbCommand().QueryComplexSingle(customMapper);
        }

        /// <summary>
        /// Executes the query and returns a DataTable.
        /// </summary>
        /// <returns>A DataTable containing the query results.</returns>
        public DataTable QueryDataTable()
        {
            return GetPreparedDbCommand().QueryDataTable();
        }

        /// <summary>
        /// Executes the query asynchronously and returns a list of entities.
        /// </summary>
        /// <param name="customMapper">A custom mapper action.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of entities.</returns>
        public Task<List<TEntity>> QueryManyAsync(Action<TEntity, IDataReader>? customMapper)
        {
            return Task.FromResult(QueryMany(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a list of entities using dynamic mapper.
        /// </summary>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of entities.</returns>
        public Task<List<TEntity>> QueryManyAsync(Action<TEntity, dynamic> customMapper)
        {
            return Task.FromResult(QueryMany(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a list using a specific collection type.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return.</typeparam>
        /// <param name="customMapper">A custom mapper action.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        public Task<TList> QueryManyAsync<TList>(Action<TEntity, IDataReader>? customMapper) where TList : IList<TEntity>
        {
            return Task.FromResult(QueryMany<TList>(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a list using a specific collection type with dynamic mapper.
        /// </summary>
        /// <typeparam name="TList">The type of collection to return.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        public Task<TList> QueryManyAsync<TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
        {
            return Task.FromResult(QueryMany<TList>(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and populates an existing list.
        /// </summary>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using IDataReader.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task QueryComplexManyAsync(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            QueryComplexMany(list, customMapper);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the query asynchronously and populates an existing list with dynamic mapper.
        /// </summary>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task QueryComplexManyAsync(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            QueryComplexMany(list, customMapper);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the query asynchronously and returns a single entity.
        /// </summary>
        /// <param name="customMapper">A custom mapper action.</param>
        /// <returns>A task representing the asynchronous operation, containing a single entity.</returns>
        public Task<TEntity> QuerySingleAsync(Action<TEntity, IDataReader>? customMapper)
        {
            return Task.FromResult(QuerySingle(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a single entity using dynamic mapper.
        /// </summary>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task representing the asynchronous operation, containing a single entity.</returns>
        public Task<TEntity> QuerySingleAsync(Action<TEntity, dynamic> customMapper)
        {
            return Task.FromResult(QuerySingle(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously with a complex custom mapper using IDataReader.
        /// </summary>
        /// <param name="customMapper">A function that maps IDataReader to entity.</param>
        /// <returns>A task representing the asynchronous operation, containing a single entity.</returns>
        public Task<TEntity> QueryComplexSingleAsync(Func<IDataReader, TEntity> customMapper)
        {
            return Task.FromResult(QueryComplexSingle(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously with a complex custom mapper using dynamic object.
        /// </summary>
        /// <param name="customMapper">A function that maps dynamic object to entity.</param>
        /// <returns>A task representing the asynchronous operation, containing a single entity.</returns>
        public Task<TEntity> QueryComplexSingleAsync(Func<dynamic, TEntity> customMapper)
        {
            return Task.FromResult(QueryComplexSingle(customMapper));
        }

        /// <summary>
        /// Executes the query asynchronously and returns a DataTable.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a DataTable.</returns>
        public Task<DataTable> QueryDataTableAsync()
        {
            return Task.FromResult(QueryDataTable());
        }
    }
}
