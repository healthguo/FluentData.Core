using Microsoft.Extensions.Configuration;

namespace FluentData.Core
{
    /// <summary>
    /// Provides a builder interface for configuring FluentData during dependency injection setup.
    /// </summary>
    public interface IFluentDataBuilder
    {
        /// <summary>
        /// Configures the connection string and database provider for the DbContext.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <param name="dbContextOptions">An optional action to further configure the <see cref="IDbContext"/>.</param>
        void ConnectionString(string connectionString, IDbProvider dbProvider, Action<IDbContext> dbContextOptions = null);

        /// <summary>
        /// Loads a connection string by name from an <see cref="IConfiguration"/> instance and configures the provider.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance to read from.</param>
        /// <param name="connectionStringName">The name of the connection string in the configuration.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <param name="dbContextOptions">An optional action to further configure the <see cref="IDbContext"/>.</param>
        void ConnectionStringName(IConfiguration configuration, string connectionStringName, IDbProvider dbProvider, Action<IDbContext> dbContextOptions = null);
    }
}