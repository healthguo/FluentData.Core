namespace FluentData.Core
{
    /// <summary>
    /// Base class for UPDATE command builders. Provides common functionality for building and executing UPDATE statements.
    /// </summary>
    internal abstract class BaseUpdateBuilder
    {
        /// <summary>
        /// Gets or sets the builder data containing command, columns, WHERE conditions, and table name.
        /// </summary>
        public BuilderData Data { get; set; }

        /// <summary>
        /// Gets or sets the actions handler for managing column values and WHERE conditions.
        /// </summary>
        protected ActionsHandler Actions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BaseUpdateBuilder"/>.
        /// </summary>
        /// <param name="provider">The database provider for SQL generation.</param>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the UPDATE statement.</param>
        public BaseUpdateBuilder(IDbProvider provider, IDbCommand command, string name)
        {
            Data = new BuilderData(command, name);
            Actions = new ActionsHandler(Data);
        }

        /// <summary>
        /// Executes the UPDATE command synchronously.
        /// </summary>
        /// <returns>The number of rows affected by the UPDATE operation.</returns>
        /// <exception cref="FluentDataException">Thrown if no columns or WHERE conditions have been added.</exception>
        public int Execute()
        {
            if (Data.Columns.Count == 0 || Data.Where.Count == 0)
                throw new FluentDataException("Columns or where filter have not yet been added.");

            Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForUpdateBuilder(Data));

            return Data.Command.Execute();
        }

        /// <summary>
        /// Executes the UPDATE command asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing the number of rows affected.</returns>
        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(Execute());
        }
    }
}
