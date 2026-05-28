namespace FluentData
{
    public partial class DbContext
    {
        /// <summary>
        /// Configures whether to ignore errors that occur during auto-mapping of query results to entities.
        /// When enabled, mapping failures will be silently ignored rather than throwing exceptions.
        /// </summary>
        /// <param name="ignoreIfAutoMapFails">True to ignore auto-mapping failures; false to throw exceptions.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext IgnoreIfAutoMapFails(bool ignoreIfAutoMapFails)
        {
            Data.IgnoreIfAutoMapFails = true;
            return this;
        }
    }
}
