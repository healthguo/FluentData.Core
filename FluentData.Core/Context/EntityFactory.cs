namespace FluentData.Core
{
    /// <summary>
    /// Default implementation of <see cref="IEntityFactory"/> that creates entity instances using <see cref="System.Activator"/>.
    /// </summary>
    public class EntityFactory : IEntityFactory
    {
        /// <summary>
        /// Creates an instance of the specified type using its parameterless constructor.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> to create.</param>
        /// <returns>A new instance of the specified type.</returns>
        public object Create(Type type)
        {
            return Activator.CreateInstance(type)!;
        }
    }
}
