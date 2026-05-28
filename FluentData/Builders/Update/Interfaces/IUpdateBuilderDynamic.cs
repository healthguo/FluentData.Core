using System;

namespace FluentData
{
    /// <summary>
    /// Provides a dynamic fluent interface for building and executing UPDATE commands using <see cref="System.Dynamic.ExpandoObject"/>.
    /// </summary>
    /// <remarks>
    /// Use this when the shape of the entity is not known at compile time. Values are read from the dynamic
    /// object and mapped to columns. Prefer strongly-typed builders when possible for compile-time safety.
    /// </remarks>
    public interface IUpdateBuilderDynamic : IExecute, IExecuteAsync
    {
        /// <summary>
        /// Gets the builder data containing command, columns, WHERE columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Gets the dynamic entity instance being used for the UPDATE operation.
        /// </summary>
        dynamic Item { get; }

        /// <summary>
        /// Automatically maps all dynamic object properties to UPDATE columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Property names to exclude from mapping.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties);

        /// <summary>
        /// Adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="propertyName">The property name on the dynamic object. Must exist on the dynamic instance.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="propertyName">The property name on the dynamic object. Must exist on the dynamic instance.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a WHERE clause condition by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="name">The property name on the dynamic object. Must exist on the dynamic instance.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic Where(string name, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="name">The property name on the dynamic object. Must exist on the dynamic instance.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic WhereIf(bool condition, string name, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Fills the UPDATE command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilderDynamic"/>.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IUpdateBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod);
    }
}