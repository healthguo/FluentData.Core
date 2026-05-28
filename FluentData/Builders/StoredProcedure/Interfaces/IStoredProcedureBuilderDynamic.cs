namespace FluentData
{
    /// <summary>
    /// Provides a dynamic fluent interface for building and executing stored procedure commands using <see cref="System.Dynamic.ExpandoObject"/>.
    /// </summary>
    /// <remarks>
    /// Use dynamic objects to build parameter lists at runtime without compile-time entity definitions.
    /// Methods on this interface mirror the strongly typed stored procedure builder but accept dynamic property values.
    /// </remarks>
    public interface IStoredProcedureBuilderDynamic : IBaseStoredProcedureBuilder
    {
        /// <summary>
        /// Automatically maps all dynamic object properties to stored procedure parameters, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Property names to exclude from mapping.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties);

        /// <summary>
        /// Adds an input parameter to the stored procedure command.
        /// </summary>
        /// <param name="name">The parameter name (with or without the '@' prefix).</param>
        /// <param name="value">The parameter value. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        IStoredProcedureBuilderDynamic Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds an output parameter to the stored procedure command. Use <see cref="IParameterValue.ParameterValue{TParameterType}(string)"/> to read the value after execution.
        /// </summary>
        /// <param name="name">The parameter name (with or without the '@' prefix).</param>
        /// <param name="parameterType">The database data type of the output parameter.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0);

        /// <summary>
        /// Configures whether the stored procedure should return multiple result sets.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result sets; false for single result set.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        IStoredProcedureBuilderDynamic UseMultiResult(bool useMultipleResultsets);
    }
}