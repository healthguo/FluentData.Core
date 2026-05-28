namespace FluentData.Core.Providers.Common.Builders
{
    /// <summary>
    /// Generates DELETE SQL statements from builder data.
    /// </summary>
    internal class DeleteBuilderSqlGenerator
    {
        /// <summary>
        /// Generates a DELETE SQL statement from the provided builder data.
        /// </summary>
        /// <param name="provider">The database provider for escaping column names.</param>
        /// <param name="parameterPrefix">The parameter prefix character (e.g., '@' for SQL Server).</param>
        /// <param name="data">The builder data containing columns for WHERE clause and table name.</param>
        /// <returns>A formatted DELETE FROM SQL statement with WHERE clause.</returns>
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

            return string.Format("delete from {0} where {1}", data.ObjectName, whereSql);
        }
    }
}
