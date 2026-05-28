using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;
using System;
using System.Data;

namespace FluentData
{
    /// <summary>
    /// Database provider for DM (Dameng Database).
    /// Supports output parameters, multiple queries, and stored procedures.
    /// Does NOT support multiple result sets.
    /// Requires explicit identity column specification.
    /// Uses RETURNING clause with output parameters for retrieving last inserted identity values.
    /// Uses ROW_NUMBER() for paging.
    /// </summary>
    public class DmProvider : IDbProvider
    {
        /// <summary>
        /// Gets the ADO.NET provider name for DM (Dameng Database).
        /// </summary>
        public string ProviderName
        {
            get
            {
                return "Dm";
            }
        }

        /// <summary>
        /// Gets a value indicating whether DM supports output parameters.
        /// DM supports output parameters.
        /// </summary>
        public bool SupportsOutputParameters
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DM supports multiple result sets in a single query.
        /// DM does NOT support multiple result sets.
        /// </summary>
        public bool SupportsMultipleResultsets
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether DM supports multiple queries in a single command.
        /// DM supports multiple queries.
        /// </summary>
        public bool SupportsMultipleQueries
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DM supports stored procedures.
        /// DM supports stored procedures.
        /// </summary>
        public bool SupportsStoredProcedures
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether DM requires explicit identity column specification for INSERT operations.
        /// DM requires explicit identity column specification.
        /// </summary>
        public bool RequiresIdentityColumn
        {
            get { return true; }
        }

        /// <summary>
        /// Creates a new database connection for DM using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for connecting to the DM database.</param>
        /// <returns>A new <see cref="IDbConnection"/> instance for DM.</returns>
        public IDbConnection CreateConnection(string connectionString)
        {
            return ConnectionFactory.CreateConnection(ProviderName, connectionString);
        }

        /// <summary>
        /// Formats a parameter name with the DM prefix (:).
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., ":Name").</returns>
        public string GetParameterName(string parameterName)
        {
            return ":" + parameterName;
        }

        /// <summary>
        /// Gets the alias format for SELECT builder column names using DM syntax.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias (e.g., "ColumnName AliasName").</returns>
        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " " + alias;
        }

        /// <summary>
        /// Generates a SELECT SQL statement with optional paging using DM syntax.
        /// Uses ROW_NUMBER() for paging.
        /// </summary>
        /// <param name="data">The select builder data containing SQL clauses.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForSelectBuilder(SelectBuilderData data)
        {
            var sql = "";
            if (data.PagingItemsPerPage == 0)
            {
                sql = "select " + data.Select;
                sql += " from " + data.From;
                if (data.WhereSql.Length > 0)
                    sql += " where " + data.WhereSql;
                if (data.GroupBy.Length > 0)
                    sql += " group by " + data.GroupBy;
                if (data.Having.Length > 0)
                    sql += " having " + data.Having;
                if (data.OrderBy.Length > 0)
                    sql += " order by " + data.OrderBy;
            }
            else if (data.PagingItemsPerPage > 0)
            {
                sql += " from " + data.From;
                if (data.WhereSql.Length > 0)
                    sql += " where " + data.WhereSql;
                if (data.GroupBy.Length > 0)
                    sql += " group by " + data.GroupBy;
                if (data.Having.Length > 0)
                    sql += " having " + data.Having;

                sql = string.Format(@"select * from
										(
											select {0}, 
												row_number() over (order by {1}) FLUENTDATA_ROWNUMBER
											{2}
										)
										where fluentdata_RowNumber between {3} and {4}
										order by fluentdata_RowNumber",
                                            data.Select,
                                            data.OrderBy,
                                            sql,
                                            data.GetFromItems(),
                                            data.GetToItems());
            }

            return sql;
        }

        /// <summary>
        /// Generates the SQL statement for an INSERT builder using DM syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, ":", data);
        }

        /// <summary>
        /// Generates the SQL statement for an UPDATE builder using DM syntax.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, ":", data);
        }

        /// <summary>
        /// Generates the SQL statement for a DELETE builder using DM syntax.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, ":", data);
        }

        /// <summary>
        /// Gets the SQL statement for a stored procedure builder using DM syntax.
        /// </summary>
        /// <param name="data">The builder data containing parameters and procedure name.</param>
        /// <returns>The stored procedure name.</returns>
        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            return data.ObjectName;
        }

        /// <summary>
        /// Maps a CLR type to the corresponding DM database data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value using RETURNING clause with output parameter.
        /// </summary>
        /// <typeparam name="T">The type of the identity value.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">The identity column name (required for DM).</param>
        /// <returns>The last inserted identity value.</returns>
        public object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
        {
            command.ParameterOut("FluentDataLastId", command.Data.Context.Data.FluentDataProvider.GetDbTypeForClrType(typeof(T)));
            command.Sql(" returning " + identityColumnName + " into :FluentDataLastId");

            object lastId = null;

            command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                command.Data.InnerCommand.ExecuteNonQuery();

                lastId = command.ParameterValue<object>("FluentDataLastId");
            });

            return lastId;
        }

        /// <summary>
        /// Called before a command is executed to configure DM-specific command settings.
        /// Enables BindByName for text commands.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        public void OnCommandExecuting(IDbCommand command)
        {
            if (command.Data.InnerCommand.CommandType == CommandType.Text)
            {
                dynamic innerCommand = command.Data.InnerCommand;
                innerCommand.BindByName = true;
            }
        }

        /// <summary>
        /// Returns the column name without escaping (DM does not require column name escaping).
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The original column name unchanged.</returns>
        public string EscapeColumnName(string name)
        {
            return name;
        }
    }
}
