namespace FluentData.Providers.Common.Builders
{
    /// <summary>
    /// Generates SQL DELETE statements from builder data.
    /// Constructs WHERE clauses using column definitions and parameter prefixes.
    /// </summary>
    internal class DeleteBuilderSqlGenerator
    {
        /// <summary>
        /// Generates a DELETE SQL statement from the provided builder data.
        /// </summary>
        /// <param name="provider">The database provider for column name escaping.</param>
        /// <param name="parameterPrefix">The parameter prefix character (e.g., "@" for SQL Server).</param>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>A formatted DELETE SQL statement.</returns>
        public string GenerateSql(IDbProvider provider, string parameterPrefix, BuilderData data)
        {
            var whereSql = "";
            foreach (var column in data.Columns)
            {
                if (whereSql.Length > 0)
                    whereSql += " and ";

                whereSql += string.Format("{0} = {1}{2}",
                                                provider.EscapeColumnName(column.ColumnName),
                                                parameterPrefix,
                                                column.ParameterName);
            }

            var sql = string.Format("delete from {0} where {1}", data.ObjectName, whereSql);
            return sql;
        }
    }
}
