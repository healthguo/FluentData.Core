using System.Linq.Expressions;
using System.Reflection;

namespace FluentData.Core
{
    /// <summary>
    /// Parses lambda expressions to extract property metadata including name, type, and value.
    /// Used by builders to convert strongly-typed property expressions into column/parameter information.
    /// </summary>
    /// <typeparam name="T">The type containing the property to parse.</typeparam>
    internal class PropertyExpressionParser<T>
    {
        private readonly object _item;

        private readonly PropertyInfo _property;

        /// <summary>
        /// Creates a new instance of <see cref="PropertyExpressionParser{T}"/>.
        /// </summary>
        /// <param name="item">The object instance to extract property values from.</param>
        /// <param name="propertyExpression">A lambda expression referencing the property (e.g., x => x.PropertyName).</param>
        public PropertyExpressionParser(object item, Expression<Func<T, object>> propertyExpression)
        {
            _item = item;
            _property = GetProperty(propertyExpression);
        }

        /// <summary>
        /// Extracts the PropertyInfo from a lambda expression.
        /// </summary>
        /// <param name="exp">The lambda expression referencing a property.</param>
        /// <returns>The PropertyInfo for the referenced property.</returns>
        /// <exception cref="ArgumentException">Thrown if the expression does not refer to a property.</exception>
        private static PropertyInfo GetProperty(Expression<Func<T, object>> exp)
        {
            PropertyInfo? result;
            if (exp.Body.NodeType == ExpressionType.Convert)
                result = ((MemberExpression)((UnaryExpression)exp.Body).Operand).Member as PropertyInfo;
            else result = ((MemberExpression)exp.Body).Member as PropertyInfo;

            if (result != null)
                return typeof(T).GetProperty(result.Name)!;

            throw new ArgumentException(string.Format("Expression '{0}' does not refer to a property.", exp.ToString()));
        }

        /// <summary>
        /// Gets the value of the parsed property from the object instance.
        /// </summary>
        public object? Value
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
        /// Gets the underlying type of the parsed property.
        /// </summary>
        public Type Type
        {
            get { return ReflectionHelper.GetPropertyType(_property); }
        }
    }
}
