namespace FluentData
{
    /// <summary>
    /// Represents a column in a builder command, containing the column name, parameter name, and value.
    /// </summary>
    public class BuilderColumn
    {
        /// <summary>
        /// Gets or sets the name of the database column.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the parameter name used in the SQL command.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the value for the column.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BuilderColumn"/>.
        /// </summary>
        /// <param name="columnName">The name of the database column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterName">The parameter name used in the SQL command.</param>
        public BuilderColumn(string columnName, object value, string parameterName)
        {
            ColumnName = columnName;
            Value = value;
            ParameterName = parameterName;
        }
    }
}
