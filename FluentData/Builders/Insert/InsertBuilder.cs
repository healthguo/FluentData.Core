using System;

namespace FluentData
{
    /// <summary>
    /// Non-generic implementation of the INSERT builder.
    /// Provides a fluent interface for adding columns and values using column names.
    /// </summary>
    internal class InsertBuilder : BaseInsertBuilder, IInsertBuilder, IInsertUpdateBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="InsertBuilder"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the INSERT statement.</param>
        internal InsertBuilder(IDbCommand command, string name)
            : base(command, name)
        {
        }

        /// <summary>
        /// Adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        public IInsertBuilder Column(string columnName, object value, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        public IInsertBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
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
        /// Fills the INSERT command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder"/>.</param>
        /// <returns>The current <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        public IInsertBuilder Fill(Action<IInsertUpdateBuilder> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}
