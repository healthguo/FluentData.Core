namespace FluentData
{
    /// <summary>
    /// Executes an INSERT command and returns the generated identity/primary key value.
    /// </summary>
    /// <remarks>
    /// The exact mechanism for retrieving the last inserted id is provider-dependent. Some providers
    /// return the value implicitly, while others require appending provider-specific SQL.
    /// </remarks>
    public interface IExecuteReturnLastId
    {
        /// <summary>
        /// Executes the INSERT command and returns the last inserted identity value.
        /// </summary>
        /// <typeparam name="T">The expected return type of the identity value (e.g., <c>int</c>, <c>long</c>, <c>Guid</c>).</typeparam>
        /// <param name="identityColumnName">Optional name of the identity column. If <c>null</c>, the provider's default behavior is used.</param>
        /// <returns>The last inserted identity value of type <typeparamref name="T"/>.</returns>
        /// <exception cref="FluentDataException">If the provider cannot return the last inserted id for the executed command.</exception>
        T ExecuteReturnLastId<T>(string identityColumnName = null);
    }
}