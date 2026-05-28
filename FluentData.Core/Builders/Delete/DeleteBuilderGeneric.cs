using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Strongly-typed implementation of the DELETE builder.
    /// Supports both expression-based and column-name-based WHERE conditions.
    /// </summary>
    /// <typeparam name="T">The entity type containing WHERE clause values.</typeparam>
    internal class DeleteBuilder<T> : BaseDeleteBuilder, IDeleteBuilder<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DeleteBuilder{T}"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="tableName">The table name for the DELETE statement.</param>
        /// <param name="item">The entity instance containing WHERE values.</param>
        public DeleteBuilder(IDbCommand command, string tableName, T item)
            : base(command, tableName)
        {
            Data.Item = item;
        }

        /// <summary>
        /// Adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Id).</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        public IDeleteBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        public IDeleteBuilder<T> WhereIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(expression, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a WHERE clause condition by column name.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        public IDeleteBuilder<T> Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition by column name.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        public IDeleteBuilder<T> WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }
    }
}
