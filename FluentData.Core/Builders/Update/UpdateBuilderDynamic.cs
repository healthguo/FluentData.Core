using System.Dynamic;

namespace FluentData.Core
{
    /// <summary>
    /// Dynamic implementation of the UPDATE builder using <see cref="ExpandoObject"/>.
    /// Provides a fluent interface for adding columns and WHERE conditions from dynamic objects.
    /// </summary>
    internal class UpdateBuilderDynamic : BaseUpdateBuilder, IUpdateBuilderDynamic, IInsertUpdateBuilderDynamic
    {
        /// <summary>
        /// Gets the dynamic entity instance being used for the UPDATE operation.
        /// </summary>
        public dynamic Item { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="UpdateBuilderDynamic"/>.
        /// </summary>
        /// <param name="dbProvider">The database provider for SQL generation.</param>
        /// <param name="command">The database command to use for execution.</param>
        /// <param name="name">The table name for the UPDATE statement.</param>
        /// <param name="item">The dynamic object containing column values.</param>
        internal UpdateBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
            : base(dbProvider, command, name)
        {
            Data.Item = item;
            Item = item;
        }

        /// <summary>
        /// Adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic Where(string columnName, object value, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
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
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a column by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="propertyName">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType, int size)
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
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType, int size)
        {
            return condition ? this.Column(propertyName, parameterType, size) : this;
        }

        /// <summary>
        /// Adds a WHERE clause condition by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="name">The property name on the dynamic object to use as WHERE condition.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic Where(string name, DataTypes parameterType, int size)
        {
            var propertyValue = ReflectionHelper.GetPropertyValueDynamic(Data.Item, name);
            Where(name, propertyValue, parameterType, size);
            return this;
        }

        /// <summary>
        /// Conditionally adds a WHERE clause condition by reading the value from the dynamic object's property.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="name">The property name on the dynamic object.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic WhereIf(bool condition, string name, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(name, parameterType, size) : this;
        }

        /// <summary>
        /// Automatically maps all dynamic object properties to UPDATE columns, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Property names to exclude from mapping.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties)
        {
            Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
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

        /// <summary>
        /// Fills the UPDATE command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilderDynamic"/>.</param>
        /// <returns>The current <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}
