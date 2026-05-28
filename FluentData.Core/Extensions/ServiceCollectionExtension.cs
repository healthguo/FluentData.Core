using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentData.Core
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register and configure FluentData in dependency injection containers.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Registers FluentData services with the DI container and returns a builder for further configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="dbContextOptions">An optional action to configure the <see cref="IDbContext"/>.</param>
        /// <returns>An <see cref="IFluentDataBuilder"/> for configuring the connection string and provider.</returns>
        public static IFluentDataBuilder AddFluentData(this IServiceCollection services, Action<IDbContext>? dbContextOptions = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            var dbContext = new DbContext();
            dbContextOptions?.Invoke(dbContext);

            services.AddSingleton(dbContext);
            services.AddSingleton<IDbContext>(dbContext);

            return new FluentDataBuilder(dbContext);
        }

        /// <summary>
        /// Registers FluentData services with the DI container and configures the connection string and provider directly.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <param name="dbContextOptions">An optional action to configure the <see cref="IDbContext"/>.</param>
        public static void AddFluentData(this IServiceCollection services, string connectionString, IDbProvider dbProvider, Action<IDbContext>? dbContextOptions = null)
        {
            services.AddFluentData(dbContextOptions).ConnectionString(connectionString, dbProvider);
        }

        /// <summary>
        /// Registers FluentData services with the DI container and loads the connection string by name from configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance to read the connection string from.</param>
        /// <param name="connectionStringName">The name of the connection string in the configuration.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <param name="dbContextOptions">An optional action to configure the <see cref="IDbContext"/>.</param>
        public static void AddFluentData(this IServiceCollection services, IConfiguration configuration, string connectionStringName, IDbProvider dbProvider, Action<IDbContext>? dbContextOptions = null)
        {
            services.AddFluentData(dbContextOptions).ConnectionStringName(configuration, connectionStringName, dbProvider);
        }

        /// <summary>
        /// Registers FluentData services with a pre-specified database provider and returns a builder for connection string configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <param name="dbContextOptions">An optional action to configure the <see cref="IDbContext"/>.</param>
        /// <returns>An <see cref="IDbProviderBuilder"/> for configuring the connection string.</returns>
        public static IDbProviderBuilder AddFluentData(this IServiceCollection services, IDbProvider dbProvider, Action<IDbContext>? dbContextOptions = null)
        {
            return new DbProviderBuilder(services.AddFluentData(dbContextOptions), dbProvider);
        }
    }
}