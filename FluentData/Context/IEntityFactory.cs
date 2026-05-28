using System;

namespace FluentData
{
    /// <summary>
    /// Factory contract used by the library to create entity and collection instances at runtime.
    /// Implementations should return a new instance of the requested <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// The default implementation uses <see cref="Activator.CreateInstance(Type)"/>.
    /// Custom implementations can provide dependency-injected instances or proxies.
    /// </remarks>
    public interface IEntityFactory
    {
        /// <summary>
        /// Create an instance of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The CLR <see cref="Type"/> to instantiate. Must be non-null and have an accessible parameterless constructor (unless the implementation supports DI).</param>
        /// <returns>
        /// A new instance of the requested type as <see cref="object"/>.
        /// Implementations MUST return an instance compatible with <paramref name="type"/> or throw an exception.
        /// </returns>
        /// <exception cref="FluentDataException">Implementation-specific errors creating the instance.</exception>
        object Create(Type type);
    }
}
