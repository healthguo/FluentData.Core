﻿using FluentData.Core.Providers.Common;
using FluentData.Core.Providers.Common.Builders;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FluentData.Core
{
    public class SqlServerProvider : IDbProvider
    {
        public string ProviderName
        {
            get
            {
                return "System.Data.SqlClient";
            }
        }

        public bool SupportsOutputParameters
        {
            get { return true; }
        }

        public bool SupportsMultipleResultsets
        {
            get { return true; }
        }

        public bool SupportsMultipleQueries
        {
            get { return true; }
        }

        public bool SupportsStoredProcedures
        {
            get { return true; }
        }

        public bool RequiresIdentityColumn
        {
            get { return false; }
        }

        public IDbConnection CreateConnection(string connectionString)
        {
            return ConnectionFactory.CreateConnection(ProviderName, connectionString);
        }

        public string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        public string GetSelectBuilderAlias(string name, string alias)
        {
            return name + " as " + alias;
        }

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

        public string GetSqlForInsertBuilder(BuilderData data)
        {
            return new InsertBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        public string GetSqlForUpdateBuilder(BuilderData data)
        {
            return new UpdateBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        public string GetSqlForDeleteBuilder(BuilderData data)
        {
            return new DeleteBuilderSqlGenerator().GenerateSql(this, "@", data);
        }

        public string GetSqlForStoredProcedureBuilder(BuilderData data)
        {
            return data.ObjectName;
        }

        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            return new DbTypeMapper().GetDbTypeForClrType(clrType);
        }

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

        public void OnCommandExecuting(IDbCommand command)
        {
        }

        public string EscapeColumnName(string name)
        {
            if (name.Contains('['))
                return name;
            return "[" + name + "]";
        }

        public void RegisterFactory(string? providerName = null)
        {
            DbProviderFactories.RegisterFactory(providerName ?? this.ProviderName, SqlClientFactory.Instance);
        }

        public DbProviderFactory GetFactory(string? providerName = null)
        {
            return DbProviderFactories.GetFactory(providerName ?? this.ProviderName);
        }

        public bool UnregisterFactory(string? providerName = null)
        {
            return DbProviderFactories.UnregisterFactory(providerName ?? this.ProviderName);
        }
    }
}
