using System.Configuration;
using System.Data.Common;

namespace FluentData
{
    public partial class DbContext
    {
        /// <summary>
        /// Configures the context using a connection string and FluentData provider.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="fluentDataProvider">The FluentData database provider.</param>
        /// <param name="providerName">The optional ADO.NET provider name. If null, uses the provider's default name.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, string providerName = null)
        {
            if (string.IsNullOrEmpty(providerName))
                providerName = fluentDataProvider.ProviderName;
            var adoNetProvider = DbProviderFactories.GetFactory(providerName);
            return ConnectionString(connectionString, fluentDataProvider, adoNetProvider);
        }

        /// <summary>
        /// Configures the context using a connection string, FluentData provider, and ADO.NET provider factory.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="fluentDataProvider">The FluentData database provider.</param>
        /// <param name="adoNetProviderFactory">The ADO.NET provider factory.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, DbProviderFactory adoNetProviderFactory)
        {
            Data.ConnectionString = connectionString;
            Data.FluentDataProvider = fluentDataProvider;
            Data.AdoNetProvider = adoNetProviderFactory;
            return this;
        }

        /// <summary>
        /// Configures the context using a named connection string from the application configuration file.
        /// </summary>
        /// <param name="connectionstringName">The name of the connection string in the .config file.</param>
        /// <param name="dbProvider">The FluentData database provider.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if no connection string with the specified name is found.</exception>
        public IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider)
        {
            var settings = ConfigurationManager.ConnectionStrings[connectionstringName] ?? throw new FluentDataException("A connectionstring with the specified name was not found in the .config file");
            ConnectionString(settings.ConnectionString, dbProvider, settings.ProviderName);
            return this;
        }
    }
}
