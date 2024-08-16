namespace FluentData.Core
{
    public interface IExecuteReturnLastId
    {
        T ExecuteReturnLastId<T>(string? identityColumnName = null);
    }
}