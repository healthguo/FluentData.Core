using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentData.Core
{
    /// <summary>
    /// Provides helper methods for reflection-based operations including property access, type checking, and expression parsing.
    /// Uses caching to improve performance for repeated property lookups.
    /// </summary>
    internal static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _cachedProperties = new();

        /// <summary>
        /// Extracts the property name from a lambda expression.
        /// </summary>
        /// <typeparam name="T">The type containing the property.</typeparam>
        /// <param name="expression">A lambda expression referencing a property (e.g., x => x.PropertyName).</param>
        /// <returns>The name of the property, including nested property paths separated by dots.</returns>
        public static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> expression)
        {
            string? propertyPath = null;
            if (expression.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression.NodeType == ExpressionType.Convert)
                    propertyPath = unaryExpression.Operand.ToString();
            }

            propertyPath ??= expression.Body.ToString();

            propertyPath = propertyPath.Replace(expression.Parameters[0] + ".", string.Empty);

            return propertyPath;
        }

        /// <summary>
        /// Extracts property names from an array of lambda expressions.
        /// </summary>
        /// <typeparam name="T">The type containing the properties.</typeparam>
        /// <param name="expressions">An array of lambda expressions referencing properties.</param>
        /// <returns>A list of property names extracted from the expressions.</returns>
        public static List<string> GetPropertyNamesFromExpressions<T>(Expression<Func<T, object>>[] expressions)
        {
            var propertyNames = new List<string>();
            foreach (var expression in expressions)
            {
                var propertyName = GetPropertyNameFromExpression(expression);
                propertyNames.Add(propertyName);
            }
            return propertyNames;
        }

        /// <summary>
        /// Gets the value of a property from an object.
        /// </summary>
        /// <param name="item">The object to get the property value from.</param>
        /// <param name="property">The property info to retrieve.</param>
        /// <returns>The property value, or null if the property is null.</returns>
        public static object? GetPropertyValue(object item, PropertyInfo? property)
        {
            return property?.GetValue(item, null);
        }

        /// <summary>
        /// Gets the value of a nested property from an object using dot-separated property path.
        /// </summary>
        /// <param name="item">The object to get the property value from.</param>
        /// <param name="propertyName">The dot-separated property path (e.g., "Address.City").</param>
        /// <returns>The nested property value, or null if any part of the path is null.</returns>
        public static object? GetPropertyValue(object? item, string propertyName)
        {
            PropertyInfo? property;
            foreach (var part in propertyName.Split('.'))
            {
                if (item == null)
                    return null;

                var type = item.GetType();

                property = type.GetProperty(part);
                if (property == null)
                    return null;

                item = GetPropertyValue(item, property);
            }
            return item;
        }

        /// <summary>
        /// Gets the value of a property from a dynamic object (ExpandoObject).
        /// </summary>
        /// <param name="item">The dynamic object to get the property value from.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The property value.</returns>
        public static object GetPropertyValueDynamic(object item, string name)
        {
            var dictionary = (IDictionary<string, object>)item;

            return dictionary[name];
        }

        /// <summary>
        /// Gets a dictionary of all properties for a type, cached for performance.
        /// </summary>
        /// <param name="type">The type to get properties for.</param>
        /// <returns>A dictionary mapping lowercase property names to their PropertyInfo objects.</returns>
        public static Dictionary<string, PropertyInfo> GetProperties(Type type)
        {
            var properties = _cachedProperties.GetOrAdd(type, BuildPropertyDictionary);

            return properties;
        }

        /// <summary>
        /// Builds a dictionary of properties for a type.
        /// </summary>
        /// <param name="type">The type to build the property dictionary for.</param>
        /// <returns>A dictionary mapping lowercase property names to their PropertyInfo objects.</returns>
        private static Dictionary<string, PropertyInfo> BuildPropertyDictionary(Type type)
        {
            var result = new Dictionary<string, PropertyInfo>();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                result.Add(property.Name.ToLower(), property);
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified item is a collection/list.
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns>True if the item implements <see cref="ICollection"/>; otherwise, false.</returns>
        public static bool IsList(object? item)
        {
            return item is ICollection;
        }

        /// <summary>
        /// Determines whether a property is a nullable type.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <returns>True if the property type is Nullable&lt;T&gt;; otherwise, false.</returns>
        public static bool IsNullable(PropertyInfo property)
        {
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;

            return false;
        }

        /// <summary>
        /// Gets the underlying type of a nullable property, or the property type itself if not nullable.
        /// </summary>
        /// <param name="property">The property to get the type for.</param>
        /// <returns>The underlying type for Nullable&lt;T&gt; properties, or the property type for non-nullable properties.</returns>
        public static Type GetPropertyType(PropertyInfo property)
        {
            if (IsNullable(property))
                return property.PropertyType.GetGenericArguments()[0];

            return property.PropertyType;
        }

        /// <summary>
        /// Gets the default value for a type.
        /// </summary>
        /// <param name="type">The type to get the default value for.</param>
        /// <returns>The default value for value types (via Activator.CreateInstance), or null for reference types.</returns>
        public static object? GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        /// <summary>
        /// Determines whether a type is a basic CLR type (enum, primitive, value type, string, or DateTime).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a basic CLR type; otherwise, false.</returns>
        public static bool IsBasicClrType(Type type)
        {
            if (type.IsEnum
                || type.IsPrimitive
                || type.IsValueType
                || type == typeof(string)
                || type == typeof(DateTime))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether a type is a custom entity class (reference type that is not a basic CLR type).
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>True if the type is a custom entity class; otherwise, false.</returns>
        public static bool IsCustomEntity<T>()
        {
            var type = typeof(T);
            if (type.IsClass && Type.GetTypeCode(type) == TypeCode.Object)
                return true;
            return false;
        }
    }
}
