using System.Data;
using System.Linq.Expressions;

namespace FluentData.Core
{
    public interface IStoredProcedureBuilder<T> : IBaseStoredProcedureBuilder
    {
        IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);

        IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);

        IStoredProcedureBuilder<T> Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0);

        IStoredProcedureBuilder<T> UseMultiResult(bool useMultipleResultsets);
    }
}