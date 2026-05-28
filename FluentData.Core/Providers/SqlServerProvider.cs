using FluentData.Core.Providers.Common;
using FluentData.Core.Providers.Common.Builders;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentData.Core
{
    /// <summary>
    /// Database provider for Microsoft SQL Server.
    /// Supports multiple result sets, output parameters, stored procedures, and multiple queries.
    /// Uses SCOPE_IDENTITY() for retrieving last inserted identity values.
    /// Uses TOP for first page and ROW_NUMBER() with CTE for subsequent pages.
    /// </summary>
    public class SqlServerProvider : IDbProvider
    {
        /// <summary>
        /// Gets the ADO.NET provider name for Microsoft SQL Server.
        /// </summary>
        public string ProviderName
        {
            get
            {
                return "Microsoft.Data.SqlClient";
            }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server supports output parameters.
        /// SQL Server supports output parameters.
        /// </summary>
        public bool SupportsOutputParameters
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server supports multiple result sets in a single query.
        /// SQL Server supports multiple result sets.
        /// </summary>
        public bool SupportsMultipleResultsets
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server supports multiple queries in a single command.
        /// SQL Server supports multiple queries.
        /// </summary>
        public bool SupportsMultipleQueries
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server supports stored procedures.
        /// SQL Server supports stored procedures.
        /// </summary>
        public bool SupportsStoredProcedures
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether SQL Server requires explicit identity column specification for INSERT operations.
        /// SQL Server does NOT require explicit identity column specification.
        /// </summary>
        public bool RequiresIdentityColumn
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a new database connection for SQL Server using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for connecting to the SQL Server database.</param>
        /// <returns>A new <see cref="IDbConnection"/> instance for SQL Server.</returns>
        public IDbConnection CreateConnection(string connectionString)
        {
            return ConnectionFactory.CreateConnection(ProviderName, connectionString);
        }

        /// <summary>
        /// Formats a parameter name with the SQL Server prefix (@).
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., "@Name").</returns>
        public string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        /// <summary>
        /// Gets the alias format for SELECT builder column names using SQL Server syntax.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias (e.g., "ColumnName AS AliasName").</returns>
        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " as " + alias;
        }

        /// <summary>
        /// Generates a SELECT SQL statement with optional paging using SQL Server syntax.
        /// Uses TOP for first page and ROW_NUMBER() with CTE for subsequent pages.
        /// </summary>
        /// <param name="data">The select builder data containing SQL clauses.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForSelectBuilder(SelectBuilderData data)
        {
            var sql = new StringBuilder();
            if (data.PagingCurrentPage == 1)
            {
                if (data.PagingItemsPerPage == 0)
                    sql.Append("select");
                else
                    sql.Append("select top " + data.PagingItemsPerPage.ToString());
                sql.Append(" " + data.Select);
                sql.Append(" from " + data.From);
                if (data.WhereSql.Length > 0)
                    sql.Append(" where " + data.WhereSql);
                if (data.GroupBy.Length > 0)
                    sql.Append(" group by " + data.GroupBy);
                if (data.Having.Length > 0)
                    sql.Append(" having " + data.Having);
                if (data.OrderBy.Length > 0)
                    sql.Append(" order by " + data.OrderBy);
                return sql.ToString();
            }
            else
            {
                sql.Append(" from " + data.From);
                if (data.WhereSql.Length > 0)
                    sql.Append(" where " + data.WhereSql);
                if (data.GroupBy.Length > 0)
                    sql.Append(" group by " + data.GroupBy);
                if (data.Having.Length > 0)
                    sql.Append(" having " + data.Having);

                var pagedSql = string.Format(@"with PagedPersons as
								(
									select top 100 percent {0}, row_number() over (order by {1}) as FLUENTDATA_ROWNUMBER
									{2}
								)
								select *
								from PagedPersons
								where fluentdata_RowNumber between {3} and {4}",
                                             data.Select,
                                             data.OrderBy,
                                             sql,
                                             data.GetFromItems(),
                                             data.GetToItems());
                return pagedSql;
            }
        }

        /// <summary>
        /// Generates the SQL statement for an INSERT builder using SQL Server syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for an UPDATE builder using SQL Server syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for a DELETE builder using SQL Server syntax.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Gets the SQL statement for a stored procedure builder using SQL Server syntax.
        /// </summary>
        /// <param name="data">The builder data containing parameters and procedure name.</param>
        /// <returns>The stored procedure name.</returns>
        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            return data.ObjectName;
        }

        /// <summary>
        /// Maps a CLR type to the corresponding SQL Server database data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value using SCOPE_IDENTITY().
        /// </summary>
        /// <typeparam name="T">The type of the identity value.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">Optional identity column name (not used for SQL Server).</param>
        /// <returns>The last inserted identity value.</returns>
        public object ExecuteReturnLastId<T>(IDbCommand command, string? identityColumnName = null)
        {
            if (command.Data.Sql[^1] != ';')
                command.Sql(";");

            command.Sql("select SCOPE_IDENTITY()");

            object? lastId = null;

            command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                lastId = command.Data.InnerCommand.ExecuteScalar();
            });

            return lastId;
        }

        /// <summary>
        /// Called before a command is executed. No provider-specific configuration is needed for SQL Server.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        public void OnCommandExecuting(IDbCommand command)
        {
        }

        /// <summary>
        /// Escapes a column name using SQL Server square bracket syntax.
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
        /// Registers the SQL Server DbProviderFactory for dynamic provider loading.
        /// </summary>
        /// <param name="providerName">Optional provider name to register. If null, uses the default provider name.</param>
        public void RegisterDbProviderFactory(string? providerName = null)
        {
            DbProviderFactories.RegisterFactory(providerName ?? this.ProviderName, "Microsoft.Data.SqlClient.SqlClientFactory, Microsoft.Data.SqlClient");
        }

        /// <summary>
        /// Gets the registered DbProviderFactory for creating SQL Server database connections and commands.
        /// </summary>
        /// <param name="providerName">Optional provider name. If null, uses the default provider name.</param>
        /// <returns>The <see cref="DbProviderFactory"/> instance for SQL Server.</returns>
        public DbProviderFactory GetDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.GetFactory(providerName ?? this.ProviderName);
        }

        /// <summary>
        /// Unregisters a previously registered SQL Server DbProviderFactory.
        /// </summary>
        /// <param name="providerName">Optional provider name to unregister. If null, unregisters the default provider name.</param>
        /// <returns>True if the provider was successfully unregistered; false otherwise.</returns>
        public bool UnregisterDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.UnregisterFactory(providerName ?? this.ProviderName);
        }
    }
}
