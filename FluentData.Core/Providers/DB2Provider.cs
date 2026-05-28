using FluentData.Core.Providers.Common;
using FluentData.Core.Providers.Common.Builders;
using System.Data;
using System.Data.Common;

namespace FluentData.Core
{
    /// <summary>
    /// Database provider for IBM DB2.
    /// Supports output parameters, multiple result sets, multiple queries, and stored procedures.
    /// Uses IDENTITY_VAL_LOCAL() for retrieving last inserted identity values.
    /// Uses LIMIT/OFFSET for paging.
    /// </summary>
    public class DB2Provider : IDbProvider
    {
        /// <summary>
        /// Gets the ADO.NET provider name for IBM DB2.
        /// </summary>
        public string ProviderName
        {
            get
            {
                return "IBM.Data.Db2";
            }
        }

        /// <summary>
        /// Gets a value indicating whether DB2 supports output parameters.
        /// DB2 supports output parameters.
        /// </summary>
        public bool SupportsOutputParameters
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DB2 supports multiple result sets in a single query.
        /// DB2 supports multiple result sets.
        /// </summary>
        public bool SupportsMultipleResultsets
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DB2 supports multiple queries in a single command.
        /// DB2 supports multiple queries.
        /// </summary>
        public bool SupportsMultipleQueries
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DB2 supports stored procedures.
        /// DB2 supports stored procedures.
        /// </summary>
        public bool SupportsStoredProcedures
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DB2 requires explicit identity column specification for INSERT operations.
        /// DB2 does NOT require explicit identity column specification.
        /// </summary>
        public bool RequiresIdentityColumn
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a new database connection for DB2 using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for connecting to the DB2 database.</param>
        /// <returns>A new <see cref="IDbConnection"/> instance for DB2.</returns>
        public IDbConnection CreateConnection(string connectionString)
        {
            return ConnectionFactory.CreateConnection(ProviderName, connectionString);
        }

        /// <summary>
        /// Formats a parameter name with the DB2 prefix (@).
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., "@Name").</returns>
        public string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        /// <summary>
        /// Gets the alias format for SELECT builder column names using DB2 syntax.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias (e.g., "ColumnName AS AliasName").</returns>
        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " as " + alias;
        }

        /// <summary>
        /// Generates a SELECT SQL statement with optional paging using DB2 syntax.
        /// Uses LIMIT/OFFSET for paging.
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
            if (data.PagingItemsPerPage > 0
                && data.PagingCurrentPage > 0)
            {
                sql += string.Format(" limit {0}, {1}", data.GetFromItems() - 1, data.GetToItems());
            }

            return sql;
        }

        /// <summary>
        /// Generates the SQL statement for an INSERT builder using DB2 syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for an UPDATE builder using DB2 syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for a DELETE builder using DB2 syntax.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Gets the SQL statement for a stored procedure builder using DB2 syntax.
        /// </summary>
        /// <param name="data">The builder data containing parameters and procedure name.</param>
        /// <returns>The stored procedure name.</returns>
        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            return data.ObjectName;
        }

        /// <summary>
        /// Maps a CLR type to the corresponding DB2 database data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value using IDENTITY_VAL_LOCAL().
        /// </summary>
        /// <typeparam name="T">The type of the identity value.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">Optional identity column name (not used for DB2).</param>
        /// <returns>The last inserted identity value.</returns>
        public object ExecuteReturnLastId<T>(IDbCommand command, string? identityColumnName = null)
        {
            if (command.Data.Sql[^1] != ';')
                command.Sql(";");

            command.Sql("select IDENTITY_VAL_LOCAL() as LastId from sysibm.sysdummy1;");

            object? lastId = null;

            command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                lastId = command.Data.InnerCommand.ExecuteScalar();
            });

            return lastId;
        }

        /// <summary>
        /// Called before a command is executed. No provider-specific configuration is needed for DB2.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        public void OnCommandExecuting(IDbCommand command)
        {
        }

        /// <summary>
        /// Returns the column name without escaping (DB2 does not require column name escaping).
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The original column name unchanged.</returns>
        public string EscapeColumnName(string name)
        {
            return name;
        }

        /// <summary>
        /// Registers the DB2 DbProviderFactory for dynamic provider loading.
        /// </summary>
        /// <param name="providerName">Optional provider name to register. If null, uses the default provider name.</param>
        public void RegisterDbProviderFactory(string? providerName = null)
        {
            DbProviderFactories.RegisterFactory(providerName ?? this.ProviderName, "IBM.Data.Db2.DB2Factory, IBM.Data.Db2");
        }

        /// <summary>
        /// Gets the registered DbProviderFactory for creating DB2 database connections and commands.
        /// </summary>
        /// <param name="providerName">Optional provider name. If null, uses the default provider name.</param>
        /// <returns>The <see cref="DbProviderFactory"/> instance for DB2.</returns>
        public DbProviderFactory GetDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.GetFactory(providerName ?? this.ProviderName);
        }

        /// <summary>
        /// Unregisters a previously registered DB2 DbProviderFactory.
        /// </summary>
        /// <param name="providerName">Optional provider name to unregister. If null, unregisters the default provider name.</param>
        /// <returns>True if the provider was successfully unregistered; false otherwise.</returns>
        public bool UnregisterDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.UnregisterFactory(providerName ?? this.ProviderName);
        }
    }
}
