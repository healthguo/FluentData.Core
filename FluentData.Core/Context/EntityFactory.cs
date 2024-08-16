namespace FluentData.Core
{
    public class EntityFactory : IEntityFactory
    {
        public object Create(Type type)
        {
            return Activator.CreateInstance(type)!;
        }
    }
}
