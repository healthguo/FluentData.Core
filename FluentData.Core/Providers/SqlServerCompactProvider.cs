using FluentData.Core.Providers.Common;
using FluentData.Core.Providers.Common.Builders;
using System.Data;
using System.Data.Common;

namespace FluentData.Core
{
    /// <summary>
    /// Database provider for SQL Server Compact Edition.
    /// Does NOT support output parameters, multiple result sets, multiple queries, or stored procedures.
    /// Uses @@identity for retrieving last inserted identity values.
    /// Uses OFFSET/FETCH for paging.
    /// </summary>
    public class SqlServerCompactProvider : IDbProvider
    {
        /// <summary>
        /// Gets the ADO.NET provider name for SQL Server Compact Edition.
        /// </summary>
        public string ProviderName
        {
            get
            {
                return "System.Data.SqlServerCe";
            }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server Compact supports output parameters.
        /// SQL Server Compact does NOT support output parameters.
        /// </summary>
        public bool SupportsOutputParameters
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server Compact supports multiple queries in a single command.
        /// SQL Server Compact does NOT support multiple queries.
        /// </summary>
        public bool SupportsMultipleQueries
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server Compact supports multiple result sets in a single query.
        /// SQL Server Compact does NOT support multiple result sets.
        /// </summary>
        public bool SupportsMultipleResultsets
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server Compact supports stored procedures.
        /// SQL Server Compact does NOT support stored procedures.
        /// </summary>
        public bool SupportsStoredProcedures
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server Compact requires explicit identity column specification for INSERT operations.
        /// SQL Server Compact does NOT require explicit identity column specification.
        /// </summary>
        public bool RequiresIdentityColumn
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a new database connection for SQL Server Compact using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for connecting to the SQL Server Compact database.</param>
        /// <returns>A new <see cref="IDbConnection"/> instance for SQL Server Compact.</returns>
        public IDbConnection CreateConnection(string connectionString)
        {
            return ConnectionFactory.CreateConnection(ProviderName, connectionString);
        }

        /// <summary>
        /// Formats a parameter name with the SQL Server Compact prefix (@).
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., "@Name").</returns>
        public string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        /// <summary>
        /// Gets the alias format for SELECT builder column names using SQL Server Compact syntax.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias (e.g., "ColumnName AS AliasName").</returns>
        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " as " + alias;
        }

        /// <summary>
        /// Generates a SELECT SQL statement with optional paging using SQL Server Compact syntax.
        /// Uses OFFSET/FETCH for paging.
        /// </summary>
        /// <param name="data">The select builder data containing SQL clauses.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForSelectBuilder(SelectBuilderData data)
        {
            var sql = "select " + data.Select;
            sql += " from " + data.From;
            if (data.WhereSql.Length > 0)
                sql += " where " + data.WhereSql;
            if (data.GroupBy.Length > 0)
                sql += " group by " + data.GroupBy;
            if (data.Having.Length > 0)
                sql += " having " + data.Having;
            if (data.OrderBy.Length > 0)
                sql += " order by " + data.OrderBy;
            if (data.PagingItemsPerPage > 0)
            {
                sql += " offset " + (data.GetFromItems() - 1) + " rows";
                if (data.PagingItemsPerPage > 0)
                    sql += " fetch next " + data.PagingItemsPerPage + " rows only";
            }

            return sql;
        }

        /// <summary>
        /// Generates the SQL statement for an INSERT builder using SQL Server Compact syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for an UPDATE builder using SQL Server Compact syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for a DELETE builder using SQL Server Compact syntax.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> as SQL Server Compact does not support stored procedures.
        /// </summary>
        /// <param name="data">The builder data.</param>
        /// <returns>Never returns (throws exception).</returns>
        /// <exception cref="NotImplementedException">Always thrown as SQL Server Compact does not support stored procedures.</exception>
        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps a CLR type to the corresponding SQL Server Compact database data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value using @@identity.
        /// </summary>
        /// <typeparam name="T">The type of the identity value.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">Optional identity column name (not used for SQL Server Compact).</param>
        /// <returns>The last inserted identity value.</returns>
        public object ExecuteReturnLastId<T>(IDbCommand command, string? identityColumnName = null)
        {
            object? lastId = null;

            command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                var recordsAffected = command.Data.InnerCommand.ExecuteNonQuery();

                if (recordsAffected > 0)
                {
                    command.Data.InnerCommand.CommandText = "select cast(@@identity as int)";

                    lastId = command.Data.InnerCommand.ExecuteScalar();
                }
            });

            return lastId;
        }

        /// <summary>
        /// Called before a command is executed. No provider-specific configuration is needed for SQL Server Compact.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        public void OnCommandExecuting(IDbCommand command)
        {

        }

        /// <summary>
        /// Escapes a column name using SQL Server Compact square bracket syntax.
        /// </summary>
        /// <param name="name">The column name to escape.</param>
        /// <returns>The escaped column name (e.g., "[Name]").</returns>
        public string EscapeColumnName(string name)
        {
            if (name.Contains('['))
                return name;
            return "[" + name + "]";
        }

        /// <summary>
        /// Registers the SQL Server Compact DbProviderFactory for dynamic provider loading.
        /// </summary>
        /// <param name="providerName">Optional provider name to register. If null, uses the default provider name.</param>
        public void RegisterDbProviderFactory(string? providerName = null)
        {
            DbProviderFactories.RegisterFactory(providerName ?? this.ProviderName, "System.Data.SqlServerCe.SqlCeProviderFactory, System.Data.SqlServerCe");
        }

        /// <summary>
        /// Gets the registered DbProviderFactory for creating SQL Server Compact database connections and commands.
        /// </summary>
        /// <param name="providerName">Optional provider name. If null, uses the default provider name.</param>
        /// <returns>The <see cref="DbProviderFactory"/> instance for SQL Server Compact.</returns>
        public DbProviderFactory GetDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.GetFactory(providerName ?? this.ProviderName);
        }

        /// <summary>
        /// Unregisters a previously registered SQL Server Compact DbProviderFactory.
        /// </summary>
        /// <param name="providerName">Optional provider name to unregister. If null, unregisters the default provider name.</param>
        /// <returns>True if the provider was successfully unregistered; false otherwise.</returns>
        public bool UnregisterDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.UnregisterFactory(providerName ?? this.ProviderName);
        }
    }
}
