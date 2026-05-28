namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Sets a custom <see cref="IEntityFactory"/> for creating entity instances during query mapping.
        /// </summary>
        /// <param name="entityFactory">The entity factory to use for creating entity instances.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext EntityFactory(IEntityFactory entityFactory)
        {
            Data.EntityFactory = entityFactory;
            return this;
        }
    }
}
