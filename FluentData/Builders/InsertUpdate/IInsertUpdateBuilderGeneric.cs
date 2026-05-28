using System;
using System.Linq.Expressions;

namespace FluentData
{
    /// <summary>
    /// Provides a strongly-typed shared fluent interface for configuring columns in both INSERT and UPDATE commands.
    /// Supports expression-based column mapping. Used by the <c>Fill</c> method to share column configuration logic.
    /// </summary>
    /// <remarks>
    /// Use this interface to centralize fill logic for insert and update operations while preserving strong typing.
    /// </remarks>
    /// <typeparam name="T">The entity type containing values to insert or update.</typeparam>
    public interface IInsertUpdateBuilder<T>
    {
        /// <summary>
        /// Gets the builder data containing command, columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Gets the entity instance being used for the operation.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Adds a column and its value to the command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a column using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}
