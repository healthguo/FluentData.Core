using FluentData.Core.Providers.Common;
using FluentData.Core.Providers.Common.Builders;
using System.Data.Common;

namespace FluentData.Core
{
    /// <summary>
    /// Database provider for Microsoft Access.
    /// Does NOT support output parameters, multiple result sets, multiple queries, or stored procedures.
    /// Uses @@Identity for retrieving last inserted identity values.
    /// Uses square brackets for escaping column names.
    /// Does NOT support paging in SELECT builder.
    /// </summary>
    public class AccessProvider : IDbProvider
    {
        /// <summary>
        /// Gets the ADO.NET provider name for Microsoft Access (OleDb).
        /// </summary>
        public string ProviderName
        {
            get
            {
                return "System.Data.OleDb";
            }
        }

        /// <summary>
        /// Gets a value indicating whether Access supports output parameters.
        /// Access does NOT support output parameters.
        /// </summary>
        public bool SupportsOutputParameters
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether Access supports multiple queries in a single command.
        /// Access does NOT support multiple queries.
        /// </summary>
        public bool SupportsMultipleQueries
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether Access supports multiple result sets in a single query.
        /// Access does NOT support multiple result sets.
        /// </summary>
        public bool SupportsMultipleResultsets
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether Access supports stored procedures.
        /// Access does NOT support stored procedures.
        /// </summary>
        public bool SupportsStoredProcedures
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether Access requires explicit identity column specification for INSERT operations.
        /// Access does NOT require explicit identity column specification.
        /// </summary>
        public bool RequiresIdentityColumn
        {
            get { return false; }
        }

        /// <summary>
        /// Formats a parameter name with the Access/OleDb prefix (@).
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., "@Name").</returns>
        public string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        /// <summary>
        /// Gets the alias format for SELECT builder column names using Access syntax.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias (e.g., "ColumnName AS AliasName").</returns>
        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " as " + alias;
        }

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> as Access does not support paging in SELECT builder.
        /// </summary>
        /// <param name="data">The select builder data.</param>
        /// <returns>Never returns (throws exception).</returns>
        /// <exception cref="NotImplementedException">Always thrown as Access does not support paging.</exception>
        public string GetSqlForSelectBuilder(SelectBuilderData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates the SQL statement for an INSERT builder using Access syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for an UPDATE builder using Access syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Generates the SQL statement for a DELETE builder using Access syntax.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> as Access does not support stored procedures.
        /// </summary>
        /// <param name="data">The builder data.</param>
        /// <returns>Never returns (throws exception).</returns>
        /// <exception cref="NotImplementedException">Always thrown as Access does not support stored procedures.</exception>
        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps a CLR type to the corresponding Access database data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value using @@Identity.
        /// </summary>
        /// <typeparam name="T">The type of the identity value.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">Optional identity column name (not used for Access).</param>
        /// <returns>The last inserted identity value.</returns>
        public object ExecuteReturnLastId<T>(IDbCommand command, string? identityColumnName = null)
        {
            object? lastId = null;

            command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                var recordsAffected = command.Data.InnerCommand.ExecuteNonQuery();

                if (recordsAffected > 0)
                {
                    command.Data.InnerCommand.CommandText = "select @@Identity";

                    lastId = command.Data.InnerCommand.ExecuteScalar();
                }
            });

            return lastId;
        }

        /// <summary>
        /// Called before a command is executed. No provider-specific configuration is needed for Access.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        public void OnCommandExecuting(IDbCommand command)
        {

        }

        /// <summary>
        /// Escapes a column name using Access square bracket syntax.
        /// </summary>
        /// <param name="name">The column name to escape.</param>
        /// <returns>The escaped column name (e.g., "[Name]").</returns>
        public string EscapeColumnName(string name)
        {
            return "[" + name + "]";
        }

        /// <summary>
        /// Checks if a column name is already escaped using Access square bracket syntax.
        /// </summary>
        /// <param name="name">The column name to check.</param>
        /// <returns>True if the column name contains square brackets, false otherwise.</returns>
        public bool IsColumnNameEscaped(string name)
        {
            if (name.Contains('['))
                return true;
            return false;
        }

        /// <summary>
        /// Registers the Access DbProviderFactory for dynamic provider loading.
        /// </summary>
        /// <param name="providerName">Optional provider name to register. If null, uses the default provider name.</param>
        public void RegisterDbProviderFactory(string? providerName = null)
        {
            DbProviderFactories.RegisterFactory(providerName ?? this.ProviderName, "System.Data.OleDb.OleDbFactory, System.Data.OleDb");
        }

        /// <summary>
        /// Gets the registered DbProviderFactory for creating Access database connections and commands.
        /// </summary>
        /// <param name="providerName">Optional provider name. If null, uses the default provider name.</param>
        /// <returns>The <see cref="DbProviderFactory"/> instance for Access.</returns>
        public DbProviderFactory GetDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.GetFactory(providerName ?? this.ProviderName);
        }

        /// <summary>
        /// Unregisters a previously registered Access DbProviderFactory.
        /// </summary>
        /// <param name="providerName">Optional provider name to unregister. If null, unregisters the default provider name.</param>
        /// <returns>True if the provider was successfully unregistered; false otherwise.</returns>
        public bool UnregisterDbProviderFactory(string? providerName = null)
        {
            return DbProviderFactories.UnregisterFactory(providerName ?? this.ProviderName);
        }
    }
}
