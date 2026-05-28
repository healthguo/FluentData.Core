using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Strongly-typed implementation of the INSERT builder.
    /// Supports expression-based column mapping and auto-mapping from entity properties.
    /// </summary>
    /// <typeparam name="T">The entity type containing values to insert.</typeparam>
    internal class InsertBuilder<T> : BaseInsertBuilder, IInsertBuilder<T>, IInsertUpdateBuilder<T>
    {
        /// <summary>
        /// Gets the entity instance being used for the INSERT operation.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="InsertBuilder{T}"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the INSERT statement.</param>
        /// <param name="item">The entity instance containing column values.</param>
        internal InsertBuilder(IDbCommand command, string name, T item)
            : base(command, name)
        {
            Data.Item = item;
            Item = item;
        }

        /// <summary>
        /// Adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a column using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            return condition ? this.Column(expression, parameterType, size) : this;
        }

        /// <summary>
        /// Fills the INSERT command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder{T}"/>.</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod)
        {
            fillMethod(this);
            return this;
        }

        /// <summary>
        /// Automatically maps all entity properties to INSERT columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Lambda expressions specifying properties to exclude from mapping (e.g., x => x.Id).</param>
        /// <returns>The current <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
        {
            Actions.AutoMapColumnsAction(ignoreProperties);
            return this;
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
    }
}
