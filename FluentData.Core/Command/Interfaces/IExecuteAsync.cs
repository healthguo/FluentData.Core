namespace FluentData.Core
{
    /// <summary>
    /// Provides asynchronous command execution capability for non-query commands (INSERT/UPDATE/DELETE).
    /// </summary>
    /// <remarks>
    /// Prefer these methods for I/O-bound scenarios to avoid blocking calling threads.
    /// The returned <see cref="Task{TResult}"/> completes once the database command has finished.
    /// </remarks>
    public interface IExecuteAsync
    {
        /// <summary>
        /// Asynchronously executes the configured command and returns the number of rows affected.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing the number of rows affected.</returns>
        /// <exception cref="FluentDataException">When execution fails due to SQL, provider, or mapping errors.</exception>
        Task<int> ExecuteAsync();
    }
}