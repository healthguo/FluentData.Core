using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentData
{
    /// <summary>
    /// Handles internal builder actions for column values, parameters, WHERE conditions, and auto-mapping.
    /// Used by all builder types (Insert, Update, Delete) to manage column and parameter operations.
    /// </summary>
    internal class ActionsHandler
{
    private bool _autoMappedAlreadyCalled = false;

    private readonly BuilderData _data;

    /// <summary>
    /// Creates a new instance of <see cref="ActionsHandler"/>.
    /// </summary>
    /// <param name="data">The builder data containing command and column information.</param>
    internal ActionsHandler(BuilderData data)
    {
        _data = data;
    }

    /// <summary>
    /// Adds a column value action for INSERT/UPDATE/DELETE operations.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="value">The value for the column.</param>
    /// <param name="parameterType">The database data type for the parameter.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    internal void ColumnValueAction(string columnName, object value, DataTypes parameterType, int size)
    {
        ColumnAction(columnName, value, typeof(object), parameterType, size);
    }

    /// <summary>
    /// Internal method to add a column with type information.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="value">The value for the column.</param>
    /// <param name="type">The CLR type of the value.</param>
    /// <param name="parameterType">The database data type.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    private void ColumnAction(string columnName, object value, Type type, DataTypes parameterType, int size)
    {
        var parameterName = columnName;

        _data.Columns.Add(new BuilderColumn(columnName, value, parameterName));

        if (parameterType == DataTypes.Object)
            parameterType = _data.Command.Data.Context.Data.FluentDataProvider.GetDbTypeForClrType(type);

        ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, size);
    }

    /// <summary>
    /// Adds a column value using a lambda expression to specify the property.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
    /// <param name="parameterType">The database data type.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    internal void ColumnValueAction<T>(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
    {
        var parser = new PropertyExpressionParser<T>(_data.Item, expression);

        ColumnAction(parser.Name, parser.Value, parser.Type, parameterType, size);
    }

    /// <summary>
    /// Adds a column value from a dynamic object property.
    /// </summary>
    /// <param name="item">The dynamic object containing the property.</param>
    /// <param name="propertyName">The name of the property to read.</param>
    /// <param name="parameterType">The database data type.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    internal void ColumnValueDynamic(ExpandoObject item, string propertyName, DataTypes parameterType, int size)
    {
        var propertyValue = (item as IDictionary<string, object>)[propertyName];

        ColumnAction(propertyName, propertyValue, typeof(object), parameterType, size);
    }

    /// <summary>
    /// Verifies that AutoMap has not been called more than once.
    /// </summary>
    /// <exception cref="FluentDataException">Thrown if AutoMap has already been called.</exception>
    private void VerifyAutoMapAlreadyCalled()
    {
        if (_autoMappedAlreadyCalled)
            throw new FluentDataException("AutoMap cannot be called more than once.");
        _autoMappedAlreadyCalled = true;
    }

    /// <summary>
    /// Auto-maps all entity properties to columns, excluding specified properties.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="ignorePropertyExpressions">Lambda expressions specifying properties to exclude.</param>
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
    /// Auto-maps all dynamic object properties to columns, excluding specified properties.
    /// </summary>
    /// <param name="ignorePropertyExpressions">Property names to exclude from mapping.</param>
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
    /// Adds an existing ADO.NET parameter to the command.
    /// </summary>
    /// <param name="parameter">The <see cref="IDataParameter"/> to add.</param>
    internal void ParameterAction(IDataParameter parameter)
    {
        _data.Command.Parameter(parameter);
    }

    /// <summary>
    /// Creates and adds a parameter with the specified configuration.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The parameter value.</param>
    /// <param name="dataType">The database data type.</param>
    /// <param name="direction">The parameter direction (input, output, etc.).</param>
    /// <param name="size">The maximum size of the parameter.</param>
    private void ParameterAction(string name, object value, DataTypes dataType, ParameterDirection direction, int size)
    {
        _data.Command.Parameter(name, value, dataType, direction, size);
    }

    /// <summary>
    /// Creates and adds an output parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="dataTypes">The database data type.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    internal void ParameterOutputAction(string name, DataTypes dataTypes, int size)
    {
        ParameterAction(name, null, dataTypes, ParameterDirection.Output, size);
    }

    /// <summary>
    /// Adds a WHERE condition to the builder.
    /// </summary>
    /// <param name="columnName">The column name for the WHERE condition.</param>
    /// <param name="value">The value to compare against.</param>
    /// <param name="parameterType">The database data type.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    internal void WhereAction(string columnName, object value, DataTypes parameterType, int size)
    {
        var parameterName = columnName;
        ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, size);

        _data.Where.Add(new BuilderColumn(columnName, value, parameterName));
    }

    /// <summary>
    /// Adds a WHERE condition using a lambda expression.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="expression">A lambda expression specifying the property.</param>
    /// <param name="parameterType">The database data type.</param>
    /// <param name="size">The maximum size of the parameter.</param>
    internal void WhereAction<T>(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
    {
        var parser = new PropertyExpressionParser<T>(_data.Item, expression);
        WhereAction(parser.Name, parser.Value, parameterType, size);
    }
}
}
