namespace FluentData.Core
{
    /// <summary>
    /// Fluent interface for building and executing stored procedure commands.
    /// Supports input/output parameter configuration and optional multi-result set handling.
    /// </summary>
    /// <remarks>
    /// Use input parameters via <see cref="Parameter(string, object, DataTypes, int)"/>,
    /// and retrieve output parameters after execution using <see cref="IParameterValue.ParameterValue{TParameterType}(string)"/>.
    /// </remarks>
    public interface IStoredProcedureBuilder : IBaseStoredProcedureBuilder
    {
        /// <summary>
        /// Adds an input parameter to the stored procedure command.
        /// </summary>
        /// <param name="name">The parameter name (with or without the provider-specific prefix such as '@').</param>
        /// <param name="value">The parameter value. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        IStoredProcedureBuilder Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds an output parameter to the stored procedure command. Use <see cref="IParameterValue.ParameterValue{TParameterType}(string)"/> to read the value after execution.
        /// </summary>
        /// <param name="name">The parameter name (with or without the provider-specific prefix).</param>
        /// <param name="parameterType">The database data type of the output parameter.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0);

        /// <summary>
        /// Configures whether the stored procedure should return multiple result sets.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result sets; false for single result set.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        IStoredProcedureBuilder UseMultiResult(bool useMultipleResultsets);
    }
}