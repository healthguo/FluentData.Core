namespace FluentData.Core
{
    public interface IInsertUpdateBuilder
    {
        BuilderData Data { get; }

        IInsertUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
        
        IInsertUpdateBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}
