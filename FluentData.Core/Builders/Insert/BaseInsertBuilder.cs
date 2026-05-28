namespace FluentData.Core
{
    /// <summary>
    /// Base class for INSERT command builders. Provides common functionality for building and executing INSERT statements.
    /// </summary>
    internal abstract class BaseInsertBuilder
    {
        /// <summary>
        /// Gets or sets the builder data containing command, columns, and table name.
        /// </summary>
        public BuilderData Data { get; set; }

        /// <summary>
        /// Gets or sets the actions handler for managing column values and parameters.
        /// </summary>
        protected ActionsHandler Actions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BaseInsertBuilder"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the INSERT statement.</param>
        public BaseInsertBuilder(IDbCommand command, string name)
        {
            Data = new BuilderData(command, name);
            Actions = new ActionsHandler(Data);
        }

        /// <summary>
        /// Prepares the command by generating the INSERT SQL and clearing any existing SQL.
        /// </summary>
        /// <returns>The prepared <see cref="IDbCommand"/> ready for execution.</returns>
        private IDbCommand GetPreparedCommand()
        {
            Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForInsertBuilder(Data));
            return Data.Command;
        }

        /// <summary>
        /// Executes the INSERT command synchronously.
        /// </summary>
        /// <returns>The number of rows affected by the INSERT operation.</returns>
        public int Execute()
        {
            return GetPreparedCommand().Execute();
        }

        /// <summary>
        /// Executes the INSERT command asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing the number of rows affected.</returns>
        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(Execute());
        }

        /// <summary>
        /// Executes the INSERT command and returns the last inserted identity value.
        /// </summary>
        /// <typeparam name="T">The type of the identity value to return.</typeparam>
        /// <param name="identityColumnName">The name of the identity column. If null, the provider's default identity column is used.</param>
        /// <returns>The last inserted identity value.</returns>
        public T ExecuteReturnLastId<T>(string? identityColumnName = null)
        {
            return GetPreparedCommand().ExecuteReturnLastId<T>(identityColumnName);
        }
    }
}
