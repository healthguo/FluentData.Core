using System;

namespace FluentData
{
    /// <summary>
    /// Non-generic implementation of the UPDATE builder.
    /// Provides a fluent interface for adding columns and WHERE conditions using column names.
    /// </summary>
    internal class UpdateBuilder : BaseUpdateBuilder, IUpdateBuilder, IInsertUpdateBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="UpdateBuilder"/>.
        /// </summary>
        /// <param name="dbProvider">The database provider for SQL generation.</param>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the UPDATE statement.</param>
        internal UpdateBuilder(IDbProvider dbProvider, IDbCommand command, string name)
            : base(dbProvider, command, name)
        {
        }

        /// <summary>
        /// Adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        public IUpdateBuilder Where(string columnName, object value, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        public IUpdateBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        public IUpdateBuilder Column(string columnName, object value, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        public IUpdateBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilder.Column"/>.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder"/> instance for method chaining.</returns>
        IInsertUpdateBuilder IInsertUpdateBuilder.Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilder.ColumnIf"/>.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder"/> instance for method chaining.</returns>
        IInsertUpdateBuilder IInsertUpdateBuilder.ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilder)this).Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Fills the UPDATE command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder"/>.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        public IUpdateBuilder Fill(Action<IInsertUpdateBuilder> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}
