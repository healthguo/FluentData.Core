using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Provides a strongly-typed fluent interface for building and executing INSERT commands.
    /// Supports expression-based column mapping and auto-mapping.
    /// </summary>
    /// <typeparam name="T">The entity type containing values to insert.</typeparam>
    /// <remarks>
    /// Prefer expression-based overloads to avoid magic strings and enable refactoring safety.
    /// After specifying columns, execute with <see cref="IExecute.Execute"/> or its async counterpart.
    /// </remarks>
    public interface IInsertBuilder<T> : IExecute, IExecuteAsync, IExecuteReturnLastId
    {
        /// <summary>
        /// Gets the builder data containing command, columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Gets the entity instance being used for the INSERT operation.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Automatically maps all entity properties to INSERT columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Lambda expressions specifying properties to exclude from mapping (e.g., x => x.Id).</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);

        /// <summary>
        /// Adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        IInsertBuilder<T> Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        IInsertBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        IInsertBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        IInsertBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Fills the INSERT command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder{T}"/>.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        IInsertBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod);
    }
}
