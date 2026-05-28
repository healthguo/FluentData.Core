namespace FluentData.Core
{
    /// <summary>
    /// Internal interface for handling type-specific query result mapping.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
    internal interface IQueryTypeHandler<TEntity>
    {
        /// <summary>
        /// Gets a value indicating whether the handler should iterate through the data reader row by row.
        /// </summary>
        bool IterateDataReader { get; }

        /// <summary>
        /// Handles the mapping of data reader results to the target entity type.
        /// </summary>
        /// <param name="customMapperReader">An optional custom mapper action using <see cref="IDataReader"/>.</param>
        /// <param name="customMapperDynamic">An optional custom mapper action using dynamic object.</param>
        /// <returns>The mapped entity object.</returns>
        object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic);
    }
}