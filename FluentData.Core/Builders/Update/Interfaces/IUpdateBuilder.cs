namespace FluentData.Core
{
    public interface IUpdateBuilder : IExecute
    {
        BuilderData Data { get; }

        IUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IUpdateBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IUpdateBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IUpdateBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IUpdateBuilder Fill(Action<IInsertUpdateBuilder> fillMethod);
    }
}