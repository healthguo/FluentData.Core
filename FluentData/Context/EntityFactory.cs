using System;

namespace FluentData
{
    /// <summary>
    /// Default implementation of <see cref="IEntityFactory"/> that creates entity instances using reflection.
    /// Uses <see cref="Activator.CreateInstance"/> to instantiate entities during query mapping.
    /// </summary>
    public class EntityFactory : IEntityFactory
    {
        /// <summary>
        /// Creates a new instance of the specified type using its parameterless constructor.
        /// </summary>
        /// <param name="type">The type of entity to create.</param>
        /// <returns>A new instance of the specified type.</returns>
        /// <exception cref="MissingMethodException">Thrown if the type has no parameterless constructor.</exception>
        public object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
