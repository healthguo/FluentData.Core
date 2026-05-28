using Microsoft.Extensions.Configuration;

namespace FluentData.Core
{
    /// <summary>
    /// Implementation of <see cref="IDbProviderBuilder"/> for configuring a specific database provider during dependency injection setup.
    /// </summary>
    public class DbProviderBuilder : IDbProviderBuilder
    {
        private readonly IFluentDataBuilder _fluentDataBuilder;
        private readonly IDbProvider _dbProvider;

        /// <summary>
        /// Creates a new instance of <see cref="DbProviderBuilder"/>.
        /// </summary>
        /// <param name="fluentDataBuilder">The <see cref="IFluentDataBuilder"/> to delegate configuration to.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> to configure.</param>
        public DbProviderBuilder(IFluentDataBuilder fluentDataBuilder, IDbProvider dbProvider)
        {
            _fluentDataBuilder = fluentDataBuilder;
            _dbProvider = dbProvider;
        }

        /// <inheritdoc/>
        public void ConnectionString(string connectionString, Action<IDbContext>? dbContextOptions = null)
        {
            _fluentDataBuilder.ConnectionString(connectionString, _dbProvider, dbContextOptions);
        }

        /// <inheritdoc/>
        public void ConnectionStringName(IConfiguration configuration, string connectionStringName, Action<IDbContext>? dbContextOptions = null)
        {
            _fluentDataBuilder.ConnectionStringName(configuration, connectionStringName, _dbProvider, dbContextOptions);
        }
    }
}
