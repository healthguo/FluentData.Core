namespace FluentData
{
    public partial class DbContext
    {
        /// <summary>
        /// Sets the command timeout in seconds for all database operations.
        /// </summary>
        /// <param name="timeout">The timeout value in seconds.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext CommandTimeout(int timeout)
        {
            Data.CommandTimeout = timeout;
            return this;
        }
    }
}
