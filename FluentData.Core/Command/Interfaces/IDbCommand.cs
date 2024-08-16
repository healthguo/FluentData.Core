using System.Data;

namespace FluentData.Core
{
    public interface IDbCommand : IExecute, IExecuteReturnLastId, IQuery, IParameterValue, IDisposable
    {
        DbCommandData Data { get; }

        IDbCommand Parameter(IDataParameter parameter);

        IDbCommand Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0);

        IDbCommand ParameterOut(string name, DataTypes parameterType, int size = 0);

        IDbCommand Parameters(params object[] parameters);

        IDbCommand Sql(string sql);

        IDbCommand ClearSql { get; }

        IDbCommand CommandType(DbCommandTypes dbCommandType);

        IDbCommand UseMultiResult(bool useMultipleResultsets);
    }
}
