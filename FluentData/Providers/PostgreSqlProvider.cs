using System;
using System.Data;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData
{
    /// <summary>
    /// Database provider for PostgreSQL.
    /// Supports multiple result sets, output parameters, stored procedures, and multiple queries.
    /// Uses lastval() for retrieving last inserted identity values.
    /// Uses LIMIT/OFFSET for paging.
    /// </summary>
    public class PostgreSqlProvider : IDbProvider
    {
        /// <summary>
        /// Gets the ADO.NET provider name for PostgreSQL.
        /// </summary>
        public string ProviderName
        { 
            get
            {
                return "Npgsql";
            } 
        }

        /// <summary>
        /// Gets a value indicating whether PostgreSQL supports output parameters.
        /// </summary>
        public bool SupportsOutputParameters
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether PostgreSQL supports multiple result sets.
        /// </summary>
        public bool SupportsMultipleResultsets
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether PostgreSQL supports multiple queries.
        /// </summary>
        public bool SupportsMultipleQueries
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether PostgreSQL supports stored procedures.
        /// </summary>
        public bool SupportsStoredProcedures
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether PostgreSQL requires explicit identity column specification.
        /// </summary>
        public bool RequiresIdentityColumn
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a new PostgreSQL connection using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <returns>A configured <see cref="IDbConnection"/> instance.</returns>
        public IDbConnection CreateConnection(string connectionString)
        {
            return ConnectionFactory.CreateConnection(ProviderName, connectionString);
        }

        /// <summary>
        /// Formats a parameter name with the PostgreSQL prefix (:).
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., ":Name").</returns>
        public string GetParameterName(string parameterName)
        {
            return ":" + parameterName;
        }

        /// <summary>
        /// Formats a column name with an alias using PostgreSQL syntax.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias (e.g., "Name as Alias").</returns>
        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " as " + alias;
        }

        /// <summary>
        /// Generates a SELECT SQL statement with optional paging using PostgreSQL LIMIT/OFFSET syntax.
        /// </summary>
        /// <param name="data">The select builder data containing SQL clauses.</param>
        /// <returns>The generated SQL statement.</returns>
        public string GetSqlForSelectBuilder(SelectBuilderData data)
        {
            var sql = "";
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
            if (data.PagingItemsPerPage > 0)
            {
                if (data.PagingItemsPerPage > 0)
                    sql += " limit " + data.PagingItemsPerPage;
                sql += " offset " + (data.GetFromItems() - 1);
            }

            
            return sql;
        }

        /// <summary>
        /// Generates an INSERT SQL statement using PostgreSQL parameter prefix.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated INSERT SQL statement.</returns>
        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, ":", data);
        }

        /// <summary>
        /// Generates an UPDATE SQL statement using PostgreSQL parameter prefix.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated UPDATE SQL statement.</returns>
        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, ":", data);
        }

        /// <summary>
        /// Generates a DELETE SQL statement using PostgreSQL parameter prefix.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated DELETE SQL statement.</returns>
        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, ":", data);
        }

        /// <summary>
        /// Returns the stored procedure name as the SQL statement.
        /// </summary>
        /// <param name="data">The builder data containing procedure name.</param>
        /// <returns>The stored procedure name.</returns>
        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            return data.ObjectName;
        }

        /// <summary>
        /// Maps a CLR type to the corresponding PostgreSQL data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value using lastval().
        /// </summary>
        /// <typeparam name="T">The type of the identity value.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">Optional identity column name (not used for PostgreSQL).</param>
        /// <returns>The last inserted identity value.</returns>
        public object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
        {
            if(command.Data.Sql[command.Data.Sql.Length - 1] != ';')
                command.Sql(";");
            command.Data.InnerCommand.CommandText += "select lastval();";

            object lastId = null;

            command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                lastId = command.Data.InnerCommand.ExecuteScalar();
            });

            return lastId;
        }

        /// <summary>
        /// Called before command execution. No additional configuration needed for PostgreSQL.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        public void OnCommandExecuting(IDbCommand command)
        {
        }

        /// <summary>
        /// Returns the column name without escaping (PostgreSQL does not require column name escaping).
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The original column name.</returns>
        public string EscapeColumnName(string name)
        {
            return name;
        }
    }
}
