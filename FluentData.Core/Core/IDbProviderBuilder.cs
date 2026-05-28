using Microsoft.Extensions.Configuration;

namespace FluentData.Core
{
    /// <summary>
    /// Provides a builder interface for configuring a specific database provider during dependency injection setup.
    /// </summary>
    public interface IDbProviderBuilder
    {
        /// <summary>
        /// Configures the connection string for the pre-specified database provider.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="dbContextOptions">An optional action to further configure the <see cref="IDbContext"/>.</param>
        void ConnectionString(string connectionString, Action<IDbContext> dbContextOptions = null);

        /// <summary>
        /// Loads a connection string by name from an <see cref="IConfiguration"/> instance for the pre-specified provider.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance to read from.</param>
        /// <param name="connectionStringName">The name of the connection string in the configuration.</param>
        /// <param name="dbContextOptions">An optional action to further configure the <see cref="IDbContext"/>.</param>
        void ConnectionStringName(IConfiguration configuration, string connectionStringName, Action<IDbContext> dbContextOptions = null);
    }
}