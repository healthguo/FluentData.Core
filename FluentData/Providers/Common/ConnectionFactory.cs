using System.Data;
using System.Data.Common;

namespace FluentData.Providers.Common
{
    /// <summary>
    /// Factory class for creating ADO.NET database connections.
    /// Uses <see cref="DbProviderFactories"/> to create provider-specific connections.
    /// </summary>
    internal class ConnectionFactory
    {
        /// <summary>
        /// Creates a new database connection using the specified provider and connection string.
        /// </summary>
        /// <param name="providerName">The ADO.NET provider name (e.g., "System.Data.SqlClient").</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A configured <see cref="IDbConnection"/> instance.</returns>
        public static IDbConnection CreateConnection(string providerName, string connectionString)
        {
            var factory = DbProviderFactories.GetFactory(providerName);

            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
