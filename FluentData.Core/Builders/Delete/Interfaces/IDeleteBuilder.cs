namespace FluentData.Core
{
    public interface IDeleteBuilder : IExecute
    {
        BuilderData Data { get; }

        IDeleteBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IDeleteBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}