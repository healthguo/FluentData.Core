using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Base class for stored procedure builders. Provides common functionality for building and executing stored procedure calls.
    /// Supports both synchronous and asynchronous execution with multiple result set handling.
    /// </summary>
    internal abstract class BaseStoredProcedureBuilder : IBaseStoredProcedureBuilder
    {
        /// <summary>
        /// Gets or sets the builder data containing command, parameters, and stored procedure name.
        /// </summary>
        public BuilderData Data { get; set; }

        /// <summary>
        /// Gets or sets the actions handler for managing parameters.
        /// </summary>
        protected ActionsHandler Actions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BaseStoredProcedureBuilder"/>.
        /// </summary>
        /// <param name="command">The database command.</param>
        /// <param name="name">The name of the stored procedure.</param>
        public BaseStoredProcedureBuilder(IDbCommand command, string name)
        {
            Data = new BuilderData(command, name);
            Actions = new ActionsHandler(Data);
        }

        /// <summary>
        /// Prepares the database command for stored procedure execution.
        /// </summary>
        /// <returns>The prepared <see cref="IDbCommand"/> instance.</returns>
        private IDbCommand GetPreparedDbCommand()
        {
            Data.Command.CommandType(DbCommandTypes.StoredProcedure);
            Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForStoredProcedureBuilder(Data));
            return Data.Command;
        }

        /// <summary>
        /// Disposes the underlying command and releases resources.
        /// </summary>
        public void Dispose()
        {
            Data.Command.Dispose();
        }

        /// <summary>
        /// Adds ADO.NET parameters and executes the stored procedure, returning a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="parameters">The ADO.NET data parameters to add.</param>
        /// <returns>A <see cref="DataTable"/> containing the query results.</returns>
        public DataTable QueryDataTable(params IDataParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                Actions.ParameterAction(parameter);
            }
            return this.QueryDataTable();
        }

        /// <summary>
        /// Adds an ADO.NET parameter to the stored procedure.
        /// </summary>
        /// <param name="parameter">The ADO.NET data parameter.</param>
        /// <returns>The current <see cref="IBaseStoredProcedureBuilder"/> instance for method chaining.</returns>
        public IBaseStoredProcedureBuilder Parameter(IDataParameter parameter)
        {
            Actions.ParameterAction(parameter);
            return this;
        }

        /// <summary>
        /// Gets the value of an output parameter by name.
        /// </summary>
        /// <param name="name">The name of the output parameter.</param>
        /// <param name="isFluentType">Whether to use FluentData type mapping.</param>
        /// <returns>The value of the output parameter.</returns>
        public object ParameterValue(string name, bool isFluentType)
        {
            return Data.Command.ParameterValue(name, isFluentType);
        }

        /// <summary>
        /// Gets the strongly-typed value of an output parameter by name.
        /// </summary>
        /// <typeparam name="TParameterType">The expected type of the parameter value.</typeparam>
        /// <param name="outputParameterName">The name of the output parameter.</param>
        /// <returns>The typed value of the output parameter.</returns>
        public TParameterType ParameterValue<TParameterType>(string outputParameterName)
        {
            return Data.Command.ParameterValue<TParameterType>(outputParameterName);
        }

        /// <summary>
        /// Executes the stored procedure synchronously.
        /// </summary>
        /// <returns>The number of rows affected by the stored procedure.</returns>
        public int Execute()
        {
            return GetPreparedDbCommand().Execute();
        }

        /// <summary>
        /// Executes the stored procedure and returns a list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A list of entities.</returns>
        public List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return GetPreparedDbCommand().QueryMany(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and returns a list of entities using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A list of entities.</returns>
        public List<TEntity> QueryMany<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return GetPreparedDbCommand().QueryMany(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and returns a custom list implementation of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A list of entities.</returns>
        public TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader>? customMapper = null) where TList : IList<TEntity>
        {
            return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and returns a custom list implementation using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <typeparam name="TList">The type of list to return.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A list of entities.</returns>
        public TList QueryMany<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
        {
            return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and populates an existing list with entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action to map data reader to entity.</param>
        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            GetPreparedDbCommand().QueryComplexMany(list, customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and populates an existing list using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            GetPreparedDbCommand().QueryComplexMany(list, customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A single entity, or default if no results.</returns>
        public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return GetPreparedDbCommand().QuerySingle(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and returns a single entity using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A single entity, or default if no results.</returns>
        public TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return GetPreparedDbCommand().QuerySingle(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure with a complex mapper function.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps <see cref="IDataReader"/> to entity.</param>
        /// <returns>A single entity.</returns>
        public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
        {
            return GetPreparedDbCommand().QueryComplexSingle(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure with a complex mapper function using dynamic object.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps dynamic object to entity.</param>
        /// <returns>A single entity.</returns>
        public TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper)
        {
            return GetPreparedDbCommand().QueryComplexSingle(customMapper);
        }

        /// <summary>
        /// Executes the stored procedure and returns results as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing the query results.</returns>
        public DataTable QueryDataTable()
        {
            return GetPreparedDbCommand().QueryDataTable();
        }

        /// <summary>
        /// Executes the stored procedure asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the number of rows affected.</returns>
        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(Execute());
        }

        /// <summary>
        /// Executes the stored procedure asynchronously and returns a list of entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A task returning a list of entities.</returns>
        public Task<List<TEntity>> QueryManyAsync<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return Task.FromResult(QueryMany(customMapper));
        }

        /// <summary>
        /// Executes the stored procedure asynchronously and returns a list of entities using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task returning a list of entities.</returns>
        public Task<List<TEntity>> QueryManyAsync<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return Task.FromResult(QueryMany(customMapper));
        }

        /// <summary>
        /// Executes the stored procedure asynchronously and returns a custom list implementation.
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
        /// Executes the stored procedure asynchronously and returns a custom list using dynamic mapper.
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
        /// Executes the stored procedure asynchronously and populates an existing list.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="list">The list to populate with results.</param>
        /// <param name="customMapper">A custom mapper action to map data reader to entity.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task QueryComplexManyAsync<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            QueryComplexMany(list, customMapper);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the stored procedure asynchronously and populates an existing list using dynamic mapper.
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

        /// <summary>
        /// Executes the stored procedure asynchronously and returns a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">An optional custom mapper action to map data reader to entity.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QuerySingleAsync<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return Task.FromResult(QuerySingle(customMapper));
        }

        /// <summary>
        /// Executes the stored procedure asynchronously and returns a single entity using dynamic mapper.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A custom mapper action using dynamic object.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QuerySingleAsync<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return Task.FromResult(QuerySingle(customMapper));
        }

        /// <summary>
        /// Executes the stored procedure asynchronously with a complex mapper function.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps <see cref="IDataReader"/> to entity.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QueryComplexSingleAsync<TEntity>(Func<IDataReader, TEntity> customMapper)
        {
            return Task.FromResult(QueryComplexSingle(customMapper));
        }

        /// <summary>
        /// Executes the stored procedure asynchronously with a complex mapper function using dynamic object.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="customMapper">A function that maps dynamic object to entity.</param>
        /// <returns>A task returning a single entity.</returns>
        public Task<TEntity> QueryComplexSingleAsync<TEntity>(Func<dynamic, TEntity> customMapper)
        {
            return Task.FromResult(QueryComplexSingle(customMapper));
        }

        /// <summary>
        /// Executes the stored procedure asynchronously and returns results as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A task returning a <see cref="DataTable"/> containing the query results.</returns>
        public Task<DataTable> QueryDataTableAsync()
        {
            return Task.FromResult(QueryDataTable());
        }
    }
}
