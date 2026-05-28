using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentData
{
    /// <summary>
    /// Parses a lambda expression to extract property metadata (name, value, type) from an object.
    /// </summary>
    /// <typeparam name="T">The type containing the property.</typeparam>
    internal class PropertyExpressionParser<T>
    {
        private readonly object _item;

        private readonly PropertyInfo _property;

        /// <summary>
        /// Creates a new instance of <see cref="PropertyExpressionParser{T}"/>.
        /// </summary>
        /// <param name="item">The object instance to parse.</param>
        /// <param name="propertyExpression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        public PropertyExpressionParser(object item, Expression<Func<T, object>> propertyExpression)
        {
            _item = item;
            _property = GetProperty(propertyExpression);
        }

        private static PropertyInfo GetProperty(Expression<Func<T, object>> exp)
        {
            PropertyInfo result;
            if (exp.Body.NodeType == ExpressionType.Convert)
                result = ((MemberExpression)((UnaryExpression)exp.Body).Operand).Member as PropertyInfo;
            else result = ((MemberExpression)exp.Body).Member as PropertyInfo;

            if (result != null)
                return typeof(T).GetProperty(result.Name);

            throw new ArgumentException(string.Format("Expression '{0}' does not refer to a property.", exp.ToString()));
        }

        /// <summary>
        /// Gets the value of the parsed property.
        /// </summary>
        public object Value
        {
            get { return ReflectionHelper.GetPropertyValue(_item, _property); }
        }

        /// <summary>
        /// Gets the name of the parsed property.
        /// </summary>
        public string Name
        {
            get { return _property.Name; }
        }

        /// <summary>
        /// Gets the type of the parsed property.
        /// </summary>
        public Type Type
        {
            get { return ReflectionHelper.GetPropertyType(_property); }
        }
    }
}
