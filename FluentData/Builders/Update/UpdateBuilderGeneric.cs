using System;
using System.Linq.Expressions;

namespace FluentData
{
    /// <summary>
    /// Strongly-typed implementation of the UPDATE builder.
    /// Supports expression-based column mapping, auto-mapping, and WHERE conditions.
    /// </summary>
    /// <typeparam name="T">The entity type containing values to update.</typeparam>
    internal class UpdateBuilder<T> : BaseUpdateBuilder, IUpdateBuilder<T>, IInsertUpdateBuilder<T>
    {
        /// <summary>
        /// Gets the entity instance being used for the UPDATE operation.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="UpdateBuilder{T}"/>.
        /// </summary>
        /// <param name="provider">The database provider for SQL generation.</param>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the UPDATE statement.</param>
        /// <param name="item">The entity instance containing column values.</param>
        internal UpdateBuilder(IDbProvider provider, IDbCommand command, string name, T item)
            : base(provider, command, name)
        {
            Data.Item = item;
            Item = item;
        }

        /// <summary>
        /// Adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Automatically maps all entity properties to UPDATE columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Lambda expressions specifying properties to exclude from mapping (e.g., x => x.Id).</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
        {
            Actions.AutoMapColumnsAction(ignoreProperties);
            return this;
        }

        /// <summary>
        /// Adds a column using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a column using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            return condition ? this.Column(expression, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.WhereAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Id).</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.WhereAction(expression, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition using a lambda expression.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> WhereIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(expression, parameterType, size) : this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilder{T}.Column(string, object, DataTypes, int)"/>.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilder{T}.ColumnIf(string, object, DataTypes, int)"/>.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilder<T>)this).Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilder{T}.Column(Expression{Func{T, object}}, DataTypes, int)"/>.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilder{T}.ColumnIf(bool, Expression{Func{T, object}}, DataTypes, int)"/>.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="expression">A lambda expression specifying the property.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder{T}"/> instance for method chaining.</returns>
        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilder<T>)this).Column(expression, parameterType, size) : this;
        }

        /// <summary>
        /// Fills the UPDATE command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder{T}"/>.</param>
        /// <returns>The current <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}
