using Microsoft.Extensions.Configuration;

namespace FluentData.Core
{
    /// <summary>
    /// Implementation of <see cref="IFluentDataBuilder"/> for configuring FluentData during dependency injection setup.
    /// </summary>
    public class FluentDataBuilder : IFluentDataBuilder
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Creates a new instance of <see cref="FluentDataBuilder"/>.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> to configure.</param>
        public FluentDataBuilder(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc/>
        public void ConnectionString(string connectionString, IDbProvider dbProvider, Action<IDbContext>? dbContextOptions = null)
        {
            if (!string.IsNullOrEmpty(connectionString) && dbProvider != null)
            {
                _dbContext.ConnectionString(connectionString, dbProvider);
            }
            dbContextOptions?.Invoke(_dbContext);
        }

        /// <inheritdoc/>
        public void ConnectionStringName(IConfiguration configuration, string connectionStringName, IDbProvider dbProvider, Action<IDbContext>? dbContextOptions = null)
        {
            if (!string.IsNullOrEmpty(connectionStringName) && dbProvider != null)
            {
                _dbContext.ConnectionStringName(configuration, connectionStringName, dbProvider);
            }
            dbContextOptions?.Invoke(_dbContext);
        }
    }
}
