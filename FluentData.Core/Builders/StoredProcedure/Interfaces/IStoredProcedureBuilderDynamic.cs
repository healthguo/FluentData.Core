using System.Data;

namespace FluentData.Core
{
    public interface IStoredProcedureBuilderDynamic : IBaseStoredProcedureBuilder
    {
        IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties);

        IStoredProcedureBuilderDynamic Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0);

        IStoredProcedureBuilderDynamic UseMultiResult(bool useMultipleResultsets);
    }
}