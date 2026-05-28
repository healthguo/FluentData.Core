namespace FluentData
{
    /// <summary>
    /// Provides synchronous command execution capability for non-query commands (INSERT/UPDATE/DELETE).
    /// </summary>
    /// <remarks>
    /// Implementations perform the database I/O synchronously and return the number of rows affected.
    /// Use the async counterpart (<see cref="IExecuteAsync"/>) in performance-sensitive or UI scenarios.
    /// </remarks>
    public interface IExecute
    {
        /// <summary>
        /// Executes the configured command synchronously and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected by the operation. Zero indicates no rows were modified.</returns>
        /// <exception cref="FluentDataException">When execution fails due to SQL, provider, or mapping errors.</exception>
        int Execute();
    }
}