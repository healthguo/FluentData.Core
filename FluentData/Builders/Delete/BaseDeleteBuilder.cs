using System.Threading.Tasks;

namespace FluentData
{
    /// <summary>
    /// Base class for DELETE command builders. Provides common functionality for building and executing DELETE statements.
    /// </summary>
    internal abstract class BaseDeleteBuilder
    {
        /// <summary>
        /// Gets or sets the builder data containing command, WHERE columns, and table name.
        /// </summary>
        public BuilderData Data { get; set; }

        /// <summary>
        /// Gets or sets the actions handler for managing WHERE conditions.
        /// </summary>
        protected ActionsHandler Actions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BaseDeleteBuilder"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the DELETE statement.</param>
        public BaseDeleteBuilder(IDbCommand command, string name)
        {
            Data = new BuilderData(command, name);
            Actions = new ActionsHandler(Data);
        }

        /// <summary>
        /// Executes the DELETE command synchronously.
        /// </summary>
        /// <returns>The number of rows affected by the DELETE operation.</returns>
        public int Execute()
        {
            Data.Command.Sql(Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForDeleteBuilder(Data));
            return Data.Command.Execute();
        }

        /// <summary>
        /// Executes the DELETE command asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing the number of rows affected.</returns>
        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(Execute());
        }
    }
}
