using System.Data;
using System.Data.Common;

namespace FluentData.Core.Providers.Common
{
    /// <summary>
    /// Factory for creating database connections using ADO.NET DbProviderFactories.
    /// </summary>
    internal class ConnectionFactory
    {
        /// <summary>
        /// Creates a database connection using the specified provider and connection string.
        /// </summary>
        /// <param name="providerName">The ADO.NET provider name (e.g., "System.Data.SqlClient").</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A configured <see cref="IDbConnection"/> instance ready to be opened.</returns>
        public static IDbConnection CreateConnection(string providerName, string connectionString)
        {
            var factory = DbProviderFactories.GetFactory(providerName);

            var connection = factory.CreateConnection()!;
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
