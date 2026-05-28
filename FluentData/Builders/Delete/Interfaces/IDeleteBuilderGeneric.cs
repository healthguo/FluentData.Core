using System;
using System.Linq.Expressions;

namespace FluentData
{
    /// <summary>
    /// Provides a strongly-typed fluent interface for building and executing DELETE commands.
    /// Supports expression-based WHERE clause mapping.
    /// </summary>
    /// <typeparam name="T">The entity type containing WHERE clause values.</typeparam>
    /// <remarks>
    /// Prefer expression-based overloads to avoid magic strings and enable refactoring safety.
    /// After specifying WHERE expressions, execute with <see cref="IExecute.Execute"/> or its async counterpart.
    /// </remarks>
    public interface IDeleteBuilder<T> : IExecute, IExecuteAsync
    {
        /// <summary>
        /// Gets the builder data containing command, WHERE columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Id).</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        IDeleteBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        IDeleteBuilder<T> WhereIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a WHERE clause condition by column name.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        IDeleteBuilder<T> Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition by column name.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        IDeleteBuilder<T> WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}