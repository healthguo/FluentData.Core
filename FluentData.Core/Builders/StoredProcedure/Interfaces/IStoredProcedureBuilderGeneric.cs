using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Provides a strongly-typed fluent interface for building and executing stored procedure commands.
    /// Supports expression-based parameter mapping and auto-mapping.
    /// </summary>
    /// <typeparam name="T">The entity type containing stored procedure parameter values.</typeparam>
    /// <remarks>
    /// Expression overloads allow selecting properties from <typeparamref name="T"/> to avoid magic strings.
    /// After configuration, execute the stored procedure and read output parameters via <see cref="IParameterValue"/>.
    /// </remarks>
    public interface IStoredProcedureBuilder<T> : IBaseStoredProcedureBuilder
    {
        /// <summary>
        /// Automatically maps all entity properties to stored procedure parameters, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Lambda expressions specifying properties to exclude from mapping (e.g., x => x.Id).</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);

        /// <summary>
        /// Adds a parameter using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds an input parameter to the stored procedure command.
        /// </summary>
        /// <param name="name">The parameter name (with or without the '@' prefix).</param>
        /// <param name="value">The parameter value. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        IStoredProcedureBuilder<T> Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds an output parameter to the stored procedure command. Use <see cref="IParameterValue.ParameterValue{TParameterType}(string)"/> to read the value after execution.
        /// </summary>
        /// <param name="name">The parameter name (with or without the '@' prefix).</param>
        /// <param name="parameterType">The database data type of the output parameter.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0);

        /// <summary>
        /// Configures whether the stored procedure should return multiple result sets.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result sets; false for single result set.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        IStoredProcedureBuilder<T> UseMultiResult(bool useMultipleResultsets);
    }
}