using System.Data;

namespace FluentData.Core
{
    public interface IStoredProcedureBuilder : IBaseStoredProcedureBuilder
    {
        IStoredProcedureBuilder Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0);

        IStoredProcedureBuilder UseMultiResult(bool useMultipleResultsets);
    }
}