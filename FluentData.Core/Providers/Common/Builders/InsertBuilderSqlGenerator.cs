namespace FluentData.Core.Providers.Common.Builders
{
    /// <summary>
    /// Generates INSERT SQL statements from builder data.
    /// </summary>
    internal class InsertBuilderSqlGenerator
    {
        /// <summary>
        /// Generates an INSERT SQL statement from the provided builder data.
        /// </summary>
        /// <param name="provider">The database provider for escaping column names.</param>
        /// <param name="parameterPrefix">The parameter prefix character (e.g., '@' for SQL Server).</param>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>A formatted INSERT INTO SQL statement.</returns>
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

            return string.Format("insert into {0}({1}) values({2})", data.ObjectName, insertSql, valuesSql);
        }
    }
}
