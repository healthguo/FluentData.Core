namespace FluentData.Core
{
    public interface IEntityFactory
    {
        object Create(Type type);
    }
}
