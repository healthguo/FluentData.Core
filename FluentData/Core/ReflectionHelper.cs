using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentData
{
    /// <summary>
    /// Provides helper methods for reflection operations, including property caching and expression parsing.
    /// </summary>
    internal static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _cachedProperties = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// Extracts the property name from a lambda expression.
        /// </summary>
        /// <typeparam name="T">The type containing the property.</typeparam>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> expression)
        {
            string propertyPath = null;
            if (expression.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression.NodeType == ExpressionType.Convert)
                    propertyPath = unaryExpression.Operand.ToString();
            }

            if (propertyPath == null)
            {
                propertyPath = expression.Body.ToString();
            }

            propertyPath = propertyPath.Replace(expression.Parameters[0] + ".", string.Empty);

            return propertyPath;
        }

        /// <summary>
        /// Extracts property names from an array of lambda expressions.
        /// </summary>
        /// <typeparam name="T">The type containing the properties.</typeparam>
        /// <param name="expressions">An array of lambda expressions specifying properties.</param>
        /// <returns>A list of property names.</returns>
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
        /// Gets the value of a property using its PropertyInfo.
        /// </summary>
        /// <param name="item">The object instance.</param>
        /// <param name="property">The PropertyInfo of the property.</param>
        /// <returns>The value of the property.</returns>
        public static object GetPropertyValue(object item, PropertyInfo property)
        {
            return property?.GetValue(item, null);
        }

        /// <summary>
        /// Gets the value of a property by its name, supporting nested properties via dot notation.
        /// </summary>
        /// <param name="item">The object instance.</param>
        /// <param name="propertyName">The property name, supporting dot notation for nested properties.</param>
        /// <returns>The value of the property, or null if not found.</returns>
        public static object GetPropertyValue(object item, string propertyName)
        {
            PropertyInfo property;
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
        /// Gets the value of a property from a dynamic object.
        /// </summary>
        /// <param name="item">The dynamic object instance.</param>
        /// <param name="name">The property name.</param>
        /// <returns>The value of the property.</returns>
        public static object GetPropertyValueDynamic(object item, string name)
        {
            var dictionary = (IDictionary<string, object>)item;

            return dictionary[name];
        }

        /// <summary>
        /// Gets all properties of a type, using cached results for performance.
        /// </summary>
        /// <param name="type">The type to get properties for.</param>
        /// <returns>A dictionary of property names (lowercase) to PropertyInfo objects.</returns>
        public static Dictionary<string, PropertyInfo> GetProperties(Type type)
        {
            var properties = _cachedProperties.GetOrAdd(type, BuildPropertyDictionary);

            return properties;
        }

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
        /// Determines whether the specified item is a list (ICollection).
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns>True if the item is an ICollection; otherwise, false.</returns>
        public static bool IsList(object item)
        {
            return item is ICollection;
        }

        /// <summary>
        /// Determines whether the specified property is nullable.
        /// </summary>
        /// <param name="property">The PropertyInfo to check.</param>
        /// <returns>True if the property is Nullable&lt;T&gt;; otherwise, false.</returns>
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
        /// <param name="property">The PropertyInfo to get the type for.</param>
        /// <returns>The underlying type.</returns>
        public static Type GetPropertyType(PropertyInfo property)
        {
            if (IsNullable(property))
                return property.PropertyType.GetGenericArguments()[0];

            return property.PropertyType;
        }

        /// <summary>
        /// Gets the default value for the specified type.
        /// </summary>
        /// <param name="type">The type to get the default value for.</param>
        /// <returns>The default value (null for reference types, default instance for value types).</returns>
        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        /// <summary>
        /// Determines whether the specified type is a basic CLR type (enum, primitive, value type, string, or DateTime).
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
        /// Determines whether the specified generic type is a custom entity class.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>True if the type is a class and not a basic CLR type; otherwise, false.</returns>
        public static bool IsCustomEntity<T>()
        {
            var type = typeof(T);
            if (type.IsClass && Type.GetTypeCode(type) == TypeCode.Object)
                return true;
            return false;
        }
    }
}
