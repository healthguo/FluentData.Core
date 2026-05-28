using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.Common;

namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Configures the context with a connection string, FluentData provider, and ADO.NET provider factory.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="dbProvider">The FluentData database provider.</param>
        /// <param name="adoNetProviderFactory">The ADO.NET <see cref="DbProviderFactory"/> for creating connections and commands.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext ConnectionString(string connectionString, IDbProvider dbProvider, DbProviderFactory adoNetProviderFactory)
        {
            Data.ConnectionString = connectionString;
            Data.FluentDataProvider = dbProvider;
            Data.AdoNetProvider = adoNetProviderFactory;
            return this;
        }

        /// <summary>
        /// Configures the context with a connection string and FluentData provider, resolving the ADO.NET provider from the provider name.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="dbProvider">The FluentData database provider.</param>
        /// <param name="providerName">The ADO.NET provider name (e.g., "System.Data.SqlClient"). If null, the provider's default is used.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext ConnectionString(string connectionString, IDbProvider dbProvider, string? providerName = null)
        {
            dbProvider.RegisterDbProviderFactory(providerName);
            var adoNetProvider = dbProvider.GetDbProviderFactory(providerName);
            return ConnectionString(connectionString, dbProvider, adoNetProvider);
        }

        /// <summary>
        /// Configures the context using a named connection string from the application configuration file (app.config/web.config).
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the configuration file.</param>
        /// <param name="dbProvider">The FluentData database provider.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the connection string name is not found in the configuration file.</exception>
        public IDbContext ConnectionStringNameFromConfigFile(string connectionStringName, IDbProvider dbProvider)
        {
            var settings = ConfigurationManager.ConnectionStrings[connectionStringName] ?? throw new FluentDataException($"connectionStringName '{connectionStringName}' not found in the *.config file");
            return ConnectionString(settings.ConnectionString, dbProvider, !string.IsNullOrEmpty(settings.ProviderName) ? settings.ProviderName : null);
        }

        /// <summary>
        /// Configures the context using a named connection string from an <see cref="IConfiguration"/> source (e.g., appsettings.json).
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="connectionStringName">The name of the connection string in the configuration.</param>
        /// <param name="dbProvider">The FluentData database provider.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the connection string name is not found in the configuration.</exception>
        public IDbContext ConnectionStringName(IConfiguration configuration, string connectionStringName, IDbProvider dbProvider)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"connectionStringName '{connectionStringName}' not found in the appsettings.json file");
            return ConnectionString(connectionString, dbProvider);
        }
    }
}
