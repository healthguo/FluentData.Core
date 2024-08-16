namespace FluentData.Core
{
    public interface IInsertBuilder : IExecute, IExecuteReturnLastId
    {
        BuilderData Data { get; }

        IInsertBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IInsertBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IInsertBuilder Fill(Action<IInsertUpdateBuilder> fillMethod);
    }
}