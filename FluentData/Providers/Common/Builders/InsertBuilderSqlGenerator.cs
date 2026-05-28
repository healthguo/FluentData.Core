namespace FluentData.Providers.Common.Builders
{
    /// <summary>
    /// Generates SQL INSERT statements from builder data.
    /// Constructs column lists and value placeholders for parameterized inserts.
    /// </summary>
    internal class InsertBuilderSqlGenerator
    {
        /// <summary>
        /// Generates an INSERT SQL statement from the provided builder data.
        /// </summary>
        /// <param name="provider">The database provider for column name escaping.</param>
        /// <param name="parameterPrefix">The parameter prefix character (e.g., "@" for SQL Server).</param>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>A formatted INSERT SQL statement.</returns>
        public string GenerateSql(IDbProvider provider, string parameterPrefix, BuilderData data)
        {
            var insertSql = "";
            var valuesSql = "";
            foreach (var column in data.Columns)
            {
                if (insertSql.Length > 0)
                {
                    insertSql += ",";
                    valuesSql += ",";
                }

                insertSql += provider.EscapeColumnName(column.ColumnName);
                valuesSql += parameterPrefix + column.ParameterName;
            }

            var sql = string.Format("insert into {0}({1}) values({2})",
                                        data.ObjectName,
                                        insertSql,
                                        valuesSql);
            return sql;
        }
    }
}
