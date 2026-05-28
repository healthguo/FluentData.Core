namespace FluentData.Core
{
    /// <summary>
    /// Non-generic implementation of the stored procedure builder.
    /// Provides a fluent interface for adding parameters using column names.
    /// </summary>
    internal class StoredProcedureBuilder : BaseStoredProcedureBuilder, IStoredProcedureBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="StoredProcedureBuilder"/>.
        /// </summary>
        /// <param name="command">The database command.</param>
        /// <param name="name">The name of the stored procedure.</param>
        internal StoredProcedureBuilder(IDbCommand command, string name)
            : base(command, name)
        {
        }

        /// <summary>
        /// Adds a parameter to the stored procedure.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder Parameter(string name, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(name, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Adds an output parameter to the stored procedure.
        /// </summary>
        /// <param name="name">The name of the output parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size)
        {
            Actions.ParameterOutputAction(name, parameterType, size);
            return this;
        }

        /// <summary>
        /// Configures whether to use multiple result sets for the stored procedure.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result sets; otherwise, false.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder UseMultiResult(bool useMultipleResultsets)
        {
            Data.Command.UseMultiResult(useMultipleResultsets);
            return this;
        }
    }
}
