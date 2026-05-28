using System;
using System.Dynamic;

namespace FluentData
{
    /// <summary>
    /// Dynamic implementation of the INSERT builder using <see cref="ExpandoObject"/>.
    /// Provides a fluent interface for adding columns from dynamic objects.
    /// </summary>
    internal class InsertBuilderDynamic : BaseInsertBuilder, IInsertBuilderDynamic, IInsertUpdateBuilderDynamic
    {
        /// <summary>
        /// Gets the dynamic entity instance being used for the INSERT operation.
        /// </summary>
        public dynamic Item { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="InsertBuilderDynamic"/>.
        /// </summary>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the INSERT statement.</param>
        /// <param name="item">The dynamic object containing column values.</param>
        internal InsertBuilderDynamic(IDbCommand command, string name, ExpandoObject item)
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
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic Column(string columnName, object value, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic Column(string propertyName, DataTypes parameterType, int size)
        {
            Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType, int size)
        {
            return condition ? this.Column(propertyName, parameterType, size) : this;
        }

        /// <summary>
        /// Automatically maps all dynamic object properties to INSERT columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Property names to exclude from mapping.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic AutoMap(params string[] ignoreProperties)
        {
            Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
            return this;
        }

        /// <summary>
        /// Fills the INSERT command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilderDynamic"/>.</param>
        /// <returns>The current <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod)
        {
            fillMethod(this);
            return this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilderDynamic.Column(string, object, DataTypes, int)"/>.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilderDynamic.ColumnIf(string, object, DataTypes, int)"/>.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilderDynamic)this).Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilderDynamic.Column(string, DataTypes, int)"/>.
        /// </summary>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName, DataTypes parameterType, int size)
        {
            Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
            return this;
        }

        /// <summary>
        /// Explicit interface implementation of <see cref="IInsertUpdateBuilderDynamic.ColumnIf(string, DataTypes, int)"/>.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilderDynamic"/> instance for method chaining.</returns>
        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.ColumnIf(bool condition, string propertyName, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilderDynamic)this).Column(propertyName, parameterType, size) : this;
        }
    }
}
