namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Sets the command timeout in seconds for all commands created by this context.
        /// </summary>
        /// <param name="timeout">The timeout value in seconds. Use 0 for no timeout.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext CommandTimeout(int timeout)
        {
            Data.CommandTimeout = timeout;
            return this;
        }
    }
}
