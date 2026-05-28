using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Provides a strongly-typed fluent interface for building and executing UPDATE commands.
    /// Supports expression-based column and WHERE clause mapping.
    /// </summary>
    /// <typeparam name="T">The entity type containing values to update.</typeparam>
    /// <remarks>
    /// Use expression-based overloads to select properties from <typeparamref name="T"/>, which helps
    /// avoid magic strings and enables refactoring safety.
    /// </remarks>
    public interface IUpdateBuilder<T> : IExecute, IExecuteAsync
    {
        /// <summary>
        /// Gets the builder data containing command, columns, WHERE columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Gets the entity instance being used for the UPDATE operation.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Automatically maps all entity properties to UPDATE columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Lambda expressions specifying properties to exclude from mapping (e.g., x => x.Id).</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);

        /// <summary>
        /// Adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Id).</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> WhereIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a WHERE clause condition by column name.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition by column name.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Fills the UPDATE command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder{T}"/>.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        IUpdateBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod);
    }
}