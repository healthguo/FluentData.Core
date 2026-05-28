namespace FluentData.Core
{
    /// <summary>
    /// Provides a dynamic fluent interface for building and executing INSERT commands using <see cref="System.Dynamic.ExpandoObject"/>.
    /// </summary>
    /// <remarks>
    /// This interface is useful when the entity shape is not known at compile time. Values are read from
    /// the dynamic object and converted to ADO.NET parameters.
    /// </remarks>
    public interface IInsertBuilderDynamic : IExecute, IExecuteAsync, IExecuteReturnLastId
    {
        /// <summary>
        /// Gets the builder data containing command, columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Gets the dynamic entity instance being used for the INSERT operation.
        /// </summary>
        dynamic Item { get; }

        /// <summary>
        /// Automatically maps all dynamic object properties to INSERT columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Property names to exclude from mapping.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        IInsertBuilderDynamic AutoMap(params string[] ignoreProperties);

        /// <summary>
        /// Adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        IInsertBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        IInsertBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="propertyName">The property name on the dynamic object. Must exist on the dynamic instance.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        IInsertBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        IInsertBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Fills the INSERT command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilderDynamic"/>.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        IInsertBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod);
    }
}