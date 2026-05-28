namespace FluentData
{
    /// <summary>
    /// Provides a dynamic shared fluent interface for configuring columns in both INSERT and UPDATE commands.
    /// Uses <see cref="System.Dynamic.ExpandoObject"/> for dynamic property access. Used by the <c>Fill</c> method.
    /// </summary>
    /// <remarks>
    /// When working with dynamic entities, this interface centralizes column configuration so callers
    /// can reuse fill logic for both insert and update operations.
    /// </remarks>
    public interface IInsertUpdateBuilderDynamic
    {
        /// <summary>
        /// Gets the builder data containing command, columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Gets the dynamic entity instance being used for the operation.
        /// </summary>
        dynamic Item { get; }

        /// <summary>
        /// Adds a column and its value to the command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}
