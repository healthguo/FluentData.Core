namespace FluentData.Core
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Appends SQL text to the command's SQL statement.
        /// </summary>
        /// <param name="sql">The SQL text to append.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        public IDbCommand Sql(string sql)
        {
            Data.Sql.Append(sql);
            return this;
        }

        /// <summary>
        /// Clears the current SQL statement buffer.
        /// </summary>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        public IDbCommand ClearSql
        {
            get
            {
                Data.Sql.Clear();
                return this;
            }
        }
    }
}
