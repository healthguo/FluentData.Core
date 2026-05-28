namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Configures whether to ignore errors during automatic mapping of data reader fields to entity properties.
        /// </summary>
        /// <param name="ignoreIfAutoMapFails">True to ignore mapping errors; false to throw exceptions on mapping failures.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext IgnoreIfAutoMapFails(bool ignoreIfAutoMapFails)
        {
            Data.IgnoreIfAutoMapFails = true;
            return this;
        }
    }
}
