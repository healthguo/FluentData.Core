using System.Data;
using System.Dynamic;
using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Handles builder actions for column values, parameters, and where clauses.
    /// Provides common functionality used across different builder types (INSERT, UPDATE, DELETE).
    /// </summary>
    internal class ActionsHandler
    {
        private bool _autoMappedAlreadyCalled = false;

        private readonly BuilderData _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsHandler"/> class.
        /// </summary>
        /// <param name="data">The builder data containing command and context information.</param>
        internal ActionsHandler(BuilderData data)
        {
            _data = data;
        }

        /// <summary>
        /// Handles a column value action with specified column name, value, and parameter type.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value to assign to the column.</param>
        /// <param name="parameterType">The data type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        internal void ColumnValueAction(string columnName, object? value, DataTypes parameterType, int size)
        {
            ColumnAction(columnName, value, typeof(object), parameterType, size);
        }

        /// <summary>
        /// Handles a column action by adding the column to the builder data and creating a corresponding parameter.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value to assign to the column.</param>
        /// <param name="type">The CLR type of the value.</param>
        /// <param name="parameterType">The data type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        private void ColumnAction(string columnName, object? value, Type type, DataTypes parameterType, int size)
        {
            var parameterName = columnName;

            _data.Columns.Add(new BuilderColumn(columnName, value, parameterName));

            if (parameterType == DataTypes.Object)
                parameterType = _data.Command.Data.Context.Data.FluentDataProvider.GetDbTypeForClrType(type);

            ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, size);
        }

        /// <summary>
        /// Handles a column value action using a lambda expression to extract property information.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="expression">The lambda expression specifying the property.</param>
        /// <param name="parameterType">The data type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        internal void ColumnValueAction<T>(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            var parser = new PropertyExpressionParser<T>(_data.Item, expression);

            ColumnAction(parser.Name, parser.Value, parser.Type, parameterType, size);
        }

        /// <summary>
        /// Handles a column value action for dynamic objects.
        /// </summary>
        /// <param name="item">The dynamic object containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="parameterType">The data type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        internal void ColumnValueDynamic(ExpandoObject item, string propertyName, DataTypes parameterType, int size)
        {
            var propertyValue = (item as IDictionary<string, object>)[propertyName];

            ColumnAction(propertyName, propertyValue, typeof(object), parameterType, size);
        }

        /// <summary>
        /// Verifies that AutoMap has not already been called to prevent duplicate mapping.
        /// </summary>
        /// <exception cref="FluentDataException">Thrown if AutoMap has already been called.</exception>
        private void VerifyAutoMapAlreadyCalled()
        {
            if (_autoMappedAlreadyCalled)
                throw new FluentDataException("AutoMap cannot be called more than once.");
            _autoMappedAlreadyCalled = true;
        }

        /// <summary>
        /// Automatically maps all properties of the specified type to columns, excluding ignored properties.
        /// </summary>
        /// <typeparam name="T">The type whose properties will be mapped.</typeparam>
        /// <param name="ignorePropertyExpressions">Optional lambda expressions specifying properties to ignore.</param>
        internal void AutoMapColumnsAction<T>(params Expression<Func<T, object>>[] ignorePropertyExpressions)
        {
            VerifyAutoMapAlreadyCalled();

            var properties = ReflectionHelper.GetProperties(_data.Item.GetType());
            var ignorePropertyNames = new HashSet<string>();
            if (ignorePropertyExpressions != null)
            {
                foreach (var ignorePropertyExpression in ignorePropertyExpressions)
                {
                    var ignorePropertyName = new PropertyExpressionParser<T>(_data.Item, ignorePropertyExpression).Name;
                    ignorePropertyNames.Add(ignorePropertyName);
                }
            }

            foreach (var property in properties)
            {
                var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Value.Name, StringComparison.CurrentCultureIgnoreCase));
                if (ignoreProperty != null)
                    continue;

                var hasIgnoreAttribute = false;
                foreach (var attribute in property.Value.GetCustomAttributes(true))
                {
                    if (attribute is IgnorePropertyAttribute)
                    {
                        hasIgnoreAttribute = true;
                        break;
                    }
                }

                if (hasIgnoreAttribute)
                    continue;

                var propertyType = ReflectionHelper.GetPropertyType(property.Value);
                var propertyValue = ReflectionHelper.GetPropertyValue(_data.Item, property.Value);
                ColumnAction(property.Value.Name, propertyValue, propertyType, DataTypes.Object, 0);
            }
        }

        /// <summary>
        /// Automatically maps all properties of a dynamic object to columns, excluding ignored properties.
        /// </summary>
        /// <param name="ignorePropertyExpressions">Optional property names to ignore.</param>
        internal void AutoMapDynamicTypeColumnsAction(params string[] ignorePropertyExpressions)
        {
            VerifyAutoMapAlreadyCalled();

            var properties = (IDictionary<string, object>)_data.Item;
            var ignorePropertyNames = new HashSet<string>();
            if (ignorePropertyExpressions != null)
            {
                foreach (var ignorePropertyExpression in ignorePropertyExpressions)
                    ignorePropertyNames.Add(ignorePropertyExpression);
            }

            foreach (var property in properties)
            {
                var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Key, StringComparison.CurrentCultureIgnoreCase));

                if (ignoreProperty == null)
                    ColumnAction(property.Key, property.Value, typeof(object), DataTypes.Object, 0);
            }
        }

        /// <summary>
        /// Handles a parameter action using an existing <see cref="IDataParameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter to add to the command.</param>
        internal void ParameterAction(IDataParameter parameter)
        {
            _data.Command.Parameter(parameter);
        }

        /// <summary>
        /// Handles a parameter action by creating a new parameter with the specified properties.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dataType">The data type of the parameter.</param>
        /// <param name="direction">The direction of the parameter (input, output, etc.).</param>
        /// <param name="size">The size of the parameter.</param>
        private void ParameterAction(string name, object? value, DataTypes dataType, ParameterDirection direction, int size)
        {
            _data.Command.Parameter(name, value, dataType, direction, size);
        }

        /// <summary>
        /// Handles an output parameter action.
        /// </summary>
        /// <param name="name">The name of the output parameter.</param>
        /// <param name="dataTypes">The data type of the output parameter.</param>
        /// <param name="size">The size of the output parameter.</param>
        internal void ParameterOutputAction(string name, DataTypes dataTypes, int size)
        {
            ParameterAction(name, null, dataTypes, ParameterDirection.Output, size);
        }

        /// <summary>
        /// Handles a where clause action by adding a column to the where clause and creating a corresponding parameter.
        /// </summary>
        /// <param name="columnName">The name of the column in the where clause.</param>
        /// <param name="value">The value for the where clause.</param>
        /// <param name="parameterType">The data type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        internal void WhereAction(string columnName, object? value, DataTypes parameterType, int size)
        {
            var parameterName = columnName;
            ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, size);

            _data.Where.Add(new BuilderColumn(columnName, value, parameterName));
        }

        /// <summary>
        /// Handles a where clause action using a lambda expression to extract property information.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="expression">The lambda expression specifying the property.</param>
        /// <param name="parameterType">The data type of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        internal void WhereAction<T>(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            var parser = new PropertyExpressionParser<T>(_data.Item, expression);
            WhereAction(parser.Name, parser.Value, parameterType, size);
        }
    }
}
