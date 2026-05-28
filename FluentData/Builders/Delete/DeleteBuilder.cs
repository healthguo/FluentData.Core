namespace FluentData
{
    /// <summary>
    /// Non-generic implementation of the DELETE builder.
    /// Provides a fluent interface for adding WHERE conditions using column names.
    /// </summary>
    internal class DeleteBuilder : BaseDeleteBuilder, IDeleteBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="DeleteBuilder"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="tableName">The table name for the DELETE statement.</param>
        public DeleteBuilder(IDbCommand command, string tableName)
            : base(command, tableName)
        {
        }

        /// <summary>
        /// Adds a WHERE clause condition to the DELETE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDeleteBuilder"/> instance for method chaining.</returns>
        public IDeleteBuilder Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition to the DELETE command.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDeleteBuilder"/> instance for method chaining.</returns>
        public IDeleteBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }
    }
}
