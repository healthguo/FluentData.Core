using System.Text;

namespace FluentData
{
    /// <summary>
    /// Contains data for executing a database command, including the SQL, reader, and execution handler.
    /// </summary>
    public class DbCommandData
    {
        /// <summary>
        /// Gets the <see cref="DbContext"/> associated with this command.
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        /// Gets the underlying ADO.NET <see cref="System.Data.IDbCommand"/> being executed.
        /// </summary>
        public System.Data.IDbCommand InnerCommand { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the command should handle multiple result sets.
        /// </summary>
        public bool UseMultipleResultsets { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDataReader"/> for reading query results.
        /// </summary>
        public IDataReader Reader { get; set; }

        /// <summary>
        /// Gets or sets the handler responsible for executing the query.
        /// </summary>
        internal ExecuteQueryHandler ExecuteQueryHandler;

        /// <summary>
        /// Gets the SQL statement being executed.
        /// </summary>
        public StringBuilder Sql { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DbCommandData"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> associated with this command.</param>
        /// <param name="innerCommand">The underlying ADO.NET <see cref="System.Data.IDbCommand"/>.</param>
        public DbCommandData(DbContext context, System.Data.IDbCommand innerCommand)
        {
            Context = context;
            InnerCommand = innerCommand;
            InnerCommand.CommandType = (System.Data.CommandType)DbCommandTypes.Text;
            Sql = new StringBuilder();
        }
    }
}
