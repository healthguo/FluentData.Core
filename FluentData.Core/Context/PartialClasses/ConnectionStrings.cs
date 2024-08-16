using System.Configuration;
using System.Data.Common;

namespace FluentData.Core
{
    public partial class DbContext
    {
        public IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, DbProviderFactory adoNetProviderFactory)
        {
            Data.ConnectionString = connectionString;
            Data.FluentDataProvider = fluentDataProvider;
            Data.AdoNetProvider = adoNetProviderFactory;
            return this;
        }

        public IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, string? providerName = null)
        {
            fluentDataProvider.RegisterFactory(providerName);
            var adoNetProvider = fluentDataProvider.GetFactory(providerName);
            return ConnectionString(connectionString, fluentDataProvider, adoNetProvider);
        }

        public IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider)
        {
            var settings = ConfigurationManager.ConnectionStrings[connectionstringName];
            if (settings == null)
                throw new FluentDataException("A connectionstring with the specified name was not found in the .config file");

            return ConnectionString(settings.ConnectionString, dbProvider, !string.IsNullOrEmpty(settings.ProviderName) ? settings.ProviderName : null);
        }
    }
}
