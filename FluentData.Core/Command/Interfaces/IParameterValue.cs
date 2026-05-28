namespace FluentData.Core
{
    /// <summary>
    /// Provides capability to retrieve parameter values after command execution, including output parameters.
    /// </summary>
    /// <remarks>
    /// Call these methods only after the command or stored procedure has been executed. Output parameters
    /// are populated by the provider during execution and can be retrieved by name. Return types are cast
    /// to the requested generic type; callers should ensure the expected CLR type matches the provider value.
    /// </remarks>
    public interface IParameterValue
    {
        /// <summary>
        /// Gets the value of an output parameter after command execution.
        /// </summary>
        /// <typeparam name="TParameterType">The expected type of the parameter value.</typeparam>
        /// <param name="outputParameterName">The name of the output parameter (with or without prefix such as '@').</param>
        /// <returns>The value of the output parameter cast to <typeparamref name="TParameterType"/>.</returns>
        TParameterType ParameterValue<TParameterType>(string outputParameterName);

        /// <summary>
        /// Gets the value of a parameter after command execution.
        /// </summary>
        /// <param name="name">The name of the parameter (with or without provider-specific prefix).</param>
        /// <param name="isFluentType">True to return the value as a FluentData-wrapped type; false for raw value.</param>
        /// <returns>The parameter value as an <see cref="object"/>. May be <c>null</c> if the value is database NULL.</returns>
        object ParameterValue(string name, bool isFluentType = true);
    }
}
