namespace FluentData.Core
{
    public interface IInsertUpdateBuilderDynamic
    {
        BuilderData Data { get; }

        dynamic Item { get; }

        IInsertUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IInsertUpdateBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IInsertUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);

        IInsertUpdateBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}
